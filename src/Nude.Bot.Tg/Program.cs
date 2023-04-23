using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Nude.API.Infrastructure.Configurations.Json;
using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Conventions;
using Nude.API.Infrastructure.Extensions;
using Nude.API.Models.Notifications;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Services.Background;
using Nude.Bot.Tg.Services.Controllers;
using Nude.Bot.Tg.Services.Handlers;
using Nude.Bot.Tg.Services.Messages.Service;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Resolvers;
using Nude.Bot.Tg.Services.Users;
using Nude.Bot.Tg.Telegram.Handlers;
using Nude.Data.Infrastructure.Contexts;
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

builder.Services.AddHostedService<BotBgService>();
builder.Services.AddSingleton<ITelegramHandler, TelegramHandler>();

#endregion

#region Endpoints & Routes

builder.Services.AddSingleton<EndpointsResolver>();
builder.Services.AddScoped<ICallbackHandler, CallbackHandler>();
builder.Services
    .AddControllers(opt =>
    {
        opt.Conventions.Add(new RoutePrefixConvention(new RouteAttribute("/callback")));
    })
    .AddNewtonsoftJson(options => options.BindOptions());

#endregion

#region Database

void ConfigureDatabase(IServiceProvider provider, DbContextOptionsBuilder opt)
{
    var connection = provider.GetRequiredService<IConfiguration>()
        .GetConnectionString("Database");
    opt.UseNpgsql(connection, x => x.MigrationsAssembly("Nude.Bot.Tg"));
    // opt.UseInMemoryDatabase("Nude.Bot.Tg");
}

builder.Services.AddDbContext<BotDbContext>(ConfigureDatabase);

#endregion

#region Services

builder.Services.AddScoped<INudeClient, NudeClient>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddSingleton<IMessagesStore, MessageStore>();

#endregion

var cancellationSource = new CancellationTokenSource(); 
Console.CancelKeyPress += (_, _) =>
{
    cancellationSource.Cancel();
};

var app = builder.Build();

app.MapControllers();

/*app.MapPost("/callback", async (context) =>
{
    var controller = new CallbackController(app.Services,context);
    await controller.ProcessCallbackAsync();
    context.Response.StatusCode = StatusCodes.Status200OK;
    await context.Response.WriteAsync("ok");
});*/

await app.RunAsync(cancellationSource.Token);