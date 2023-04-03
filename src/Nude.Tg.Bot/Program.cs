using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nude.API.Data.Contexts;
using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Extensions;
using Nude.Tg.Bot.Clients.Nude;
using Nude.Tg.Bot.Clients.Telegraph;
using Nude.Tg.Bot.Extensions;
using Nude.Tg.Bot.Http;
using Nude.Tg.Bot.Http.Routes;
using Nude.Tg.Bot.Services.Convert;
using Nude.Tg.Bot.Services.Manga;
using Nude.Tg.Bot.Services.Messages.Store;
using Nude.Tg.Bot.Services.Messages.Telegram;
using Nude.Tg.Bot.Services.Resolvers;
using Nude.Tg.Bot.Services.Workers;
using Nude.Tg.Bot.Telegram;
using Nude.Tg.Bot.Telegram.Endpoints.Base;
using Nude.Tg.Bot.Telegram.Endpoints.Update;
using Nude.Tg.Bot.Telegram.Handlers;
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
    services.AddSingleton<ITelegraphClient, DefaultTelegraphClient>();

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
        opt.UseNpgsql(connection, x => x.MigrationsAssembly("Nude.Tg.Bot"));
    }

    services.AddDbContext<BotDbContext>(ConfigureDatabase);
    services.AddDbContextFactory<BotDbContext>(ConfigureDatabase); // NOTE: commit this row to create new migration

    #endregion

    #region Background

    services.AddBackgroundWorker<ConvertingBackgroundWorker>();

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