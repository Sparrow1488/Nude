using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nude.API.Infrastructure.Clients.Telegraph;
using Nude.Data.Infrastructure.Contexts;
using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Converters;
using Nude.API.Models.Notifications;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Extensions;
using Nude.Bot.Tg.Http;
using Nude.Bot.Tg.Http.Routes;
using Nude.Bot.Tg.Services.Background;
using Nude.Bot.Tg.Services.Convert;
using Nude.Bot.Tg.Services.Manga;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Messages.Telegram;
using Nude.Bot.Tg.Services.Resolvers;
using Nude.Bot.Tg.Telegram.Handlers;
using Serilog;
using Serilog.Events;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

#region Configuration

builder.Host.ConfigureHostConfiguration(
    x => x.AddUserSecrets(Assembly.GetExecutingAssembly()));

#endregion

#region Logger

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .CreateLogger();

#endregion

#region Bot

builder.Services.AddSingleton<ITelegramBotClient>(x =>
{
    var token = x.GetRequiredService<IConfiguration>()[BotDefaults.TelegramAccessTokenSection];
    return new TelegramBotClient(token);
});

#endregion

#region Clients

builder.Services.AddScoped<INudeClient, NudeClient>();
builder.Services.AddSingleton<ITelegraphClient, DefaultTelegraphClient>();

#endregion

#region Http Routes

builder.Services.AddScoped<CallbackRoute>();

#endregion

#region Endpoints

builder.Services.AddSingleton<EndpointsResolver>();
builder.Services.AddTelegramEndpoints();

#endregion

#region Telegram Handlers

builder.Services.AddSingleton<ITelegramHandler, TelegramHandler>();

#endregion

#region Database

void ConfigureDatabase(IServiceProvider provider, DbContextOptionsBuilder opt)
{
    var connection = provider.GetRequiredService<IConfiguration>()
        .GetConnectionString("Database");
    opt.UseNpgsql(connection, x => x.MigrationsAssembly("Nude.Bot.Tg"));
}

builder.Services.AddDbContext<FixedBotDbContext>(ConfigureDatabase);
builder.Services.AddDbContext<BotDbContext>(ConfigureDatabase);

// builder.Services.AddDbContextFactory<BotDbContext>(ConfigureDatabase); // NOTE: commit this row to create new migration

#endregion

#region Background
builder.Services.AddHostedService<BotBgService>();

#endregion

#region Services

builder.Services.AddScoped<ITelegraphMangaService, TelegraphMangaService>();
builder.Services.AddScoped<IConvertTicketsService, ConvertTicketsService>();
builder.Services.AddScoped<IMessagesStore, MessageStore>();
builder.Services.AddScoped<ITelegramMessagesService, TelegramMessagesService>();

#endregion

var cancellationSource = new CancellationTokenSource(); 
Console.CancelKeyPress += (_, _) =>
{
    cancellationSource.Cancel();
};

var app = builder.Build();

app.MapPost("/callback", async ctx =>
{
    using var content = new StreamContent(ctx.Request.Body);
    var subjectJson = await content.ReadAsStringAsync();
    
    var jsonSettings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        ContractResolver = new DefaultContractResolver
        {
            // NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };
    jsonSettings.Converters.Add(new NotificationDetailsConverter());
    jsonSettings.Converters.Add(new FormattedContentResponseConverter());
    
    // TODO: ОТПРАВЛЯТЬ С АПИ ОТВЕТ В SNAKE CASE
    var subject = JsonConvert.DeserializeObject<NotificationSubject>(subjectJson, jsonSettings);

    var callbackRoute = app.Services.GetRequiredService<CallbackRoute>();
    await callbackRoute.OnCallbackAsync(subject!);
    
    ctx.Response.StatusCode = StatusCodes.Status200OK;
    await ctx.Response.WriteAsync("ok");
});

await app.RunAsync(cancellationSource.Token);
await HttpServer.StartListenAsync(app.Services, cancellationSource.Token);
