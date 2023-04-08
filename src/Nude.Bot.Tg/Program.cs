using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nude.Data.Infrastructure.Contexts;
using Nude.API.Infrastructure.Constants;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Extensions;
using Nude.Bot.Tg.Http;
using Nude.Bot.Tg.Http.Routes;
using Nude.Bot.Tg.Services.Convert;
using Nude.Bot.Tg.Services.Manga;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Messages.Telegram;
using Nude.Bot.Tg.Services.Resolvers;
using Nude.Bot.Tg.Telegram;
using Nude.Bot.Tg.Telegram.Handlers;
using Serilog;
using Serilog.Events;
using Telegram.Bot;

var builder = Host.CreateDefaultBuilder();

#region Configuration

builder.ConfigureHostConfiguration(x => x.AddUserSecrets(Assembly.GetExecutingAssembly()));

#endregion

#region Logger

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .CreateLogger();

#endregion

builder.ConfigureServices(services =>
{
    #region Bot

    services.AddSingleton<ITelegramBotClient>(x =>
    {
        var token = x.GetRequiredService<IConfiguration>()[BotDefaults.TelegramAccessTokenSection];
        return new TelegramBotClient(token);
    });

    #endregion
    
    #region Clients

    services.AddScoped<INudeClient, NudeClient>();
    // services.AddSingleton<ITelegraphClient, DefaultTelegraphClient>();

    #endregion

    #region Http Routes

    services.AddScoped<CallbackRoute>();

    #endregion
    
    #region Endpoints

    services.AddSingleton<EndpointsResolver>();
    services.AddTelegramEndpoints();

    #endregion

    #region Telegram Handlers

    services.AddSingleton<ITelegramHandler, TelegramHandler>();

    #endregion

    #region Database

    void ConfigureDatabase(IServiceProvider provider, DbContextOptionsBuilder opt)
    {
        var connection = provider.GetRequiredService<IConfiguration>()
            .GetConnectionString("Database");
        opt.UseNpgsql(connection, x => x.MigrationsAssembly("Nude.Bot.Tg"));
    }

    services.AddDbContext<BotDbContext>(ConfigureDatabase);
    services.AddDbContextFactory<BotDbContext>(ConfigureDatabase); // NOTE: commit this row to create new migration

    #endregion

    #region Background

    // services.AddBackgroundWorker<ConvertingBackgroundWorker>();

    #endregion

    #region Services

    services.AddScoped<ITelegraphMangaService, TelegraphMangaService>();
    services.AddScoped<IConvertTicketsService, ConvertTicketsService>();
    services.AddScoped<IMessagesStore, MessageStore>();
    services.AddScoped<ITelegramMessagesService, TelegramMessagesService>();

    #endregion

}).UseSerilog(Log.Logger);

var cancellationSource = new CancellationTokenSource(); 
Console.CancelKeyPress += (_, _) =>
{
    cancellationSource.Cancel();
};

var host = builder.Build();
await host.StartAsync(cancellationSource.Token);

await BotInitializer.StartReceiveAsync(host.Services, cancellationSource.Token);
await HttpServer.StartListenAsync(host.Services, cancellationSource.Token);