using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nude.API.Infrastructure.Configurations.Json;
using Nude.Data.Infrastructure.Contexts;
using Nude.API.Infrastructure.Constants;
using Nude.API.Models.Notifications;
using Nude.API.Models.Notifications.Details;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Extensions;
using Nude.Bot.Tg.Http.Routes;
using Nude.Bot.Tg.Services.Background;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Resolvers;
using Nude.Bot.Tg.Telegram.Handlers;
using Serilog;
using Serilog.Events;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureHostConfiguration(
    x => x.AddUserSecrets(Assembly.GetExecutingAssembly()));

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

builder.Services.AddHostedService<BotBgService>();
builder.Services.AddSingleton<ITelegramHandler, TelegramHandler>();

#endregion

#region Endpoints & Routes

builder.Services.AddSingleton<EndpointsResolver>();
builder.Services.AddTelegramEndpoints();
builder.Services.AddScoped<CallbackRoute>();

#endregion

#region Database

void ConfigureDatabase(IServiceProvider provider, DbContextOptionsBuilder opt)
{
    var connection = provider.GetRequiredService<IConfiguration>()
        .GetConnectionString("Database");
    opt.UseNpgsql(connection, x => x.MigrationsAssembly("Nude.Bot.Tg"));
}

builder.Services.AddDbContext<BotDbContext>(ConfigureDatabase);

#endregion

#region Services

builder.Services.AddScoped<INudeClient, NudeClient>();
builder.Services.AddScoped<IMessagesStore, MessageStore>();

#endregion


var subj = new NotificationSubject
{
    EventDetails = new FormatTicketProgressDetails
    {
        CurrentImage = 100,
        TotalImages = 500,
        TicketId = 123
    }
};

var settings = JsonSettingsProvider.Create();
var json = JsonConvert.SerializeObject(subj, settings);

var result = JsonConvert.DeserializeObject<NotificationSubject>(json, settings);


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

    var subject = JsonConvert.DeserializeObject<NotificationSubject>(
        subjectJson, 
        JsonSettingsProvider.Create()
    );

    var callbackRoute = app.Services.GetRequiredService<CallbackRoute>();
    await callbackRoute.OnCallbackAsync(subject!);
    
    ctx.Response.StatusCode = StatusCodes.Status200OK;
    await ctx.Response.WriteAsync("ok");
});

await app.RunAsync(cancellationSource.Token);