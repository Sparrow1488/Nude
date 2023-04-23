using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Extensions;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Services.Background;
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

#region Routing

builder.Services.AddSingleton<EndpointsResolver>();
builder.Services.AddScoped<ICallbackHandler, CallbackHandler>();

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => options.BindOptions());

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

await app.RunAsync(cancellationSource.Token);