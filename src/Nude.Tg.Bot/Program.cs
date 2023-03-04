using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nude.API.Data.Contexts;
using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Extensions;
using Nude.Tg.Bot.Clients.Nude;
using Nude.Tg.Bot.Clients.Telegraph;
using Nude.Tg.Bot.Http.Routes;
using Nude.Tg.Bot.Initializers;
using Nude.Tg.Bot.Services.Background;
using Nude.Tg.Bot.Services.Convert;
using Nude.Tg.Bot.Services.Manga;
using Nude.Tg.Bot.Services.Messages;
using Nude.Tg.Bot.Services.Resolvers;
using Nude.Tg.Bot.Telegram.Endpoints.Base;
using Nude.Tg.Bot.Telegram.Endpoints.Update;
using Nude.Tg.Bot.Telegram.Handlers;
using Serilog;
using Serilog.Events;
using Telegram.Bot;

var builder = Host.CreateDefaultBuilder();

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
    
    services.AddScoped<TelegramUpdateEndpoint, StartEndpoint>();
    services.AddScoped<TelegramUpdateEndpoint, MenuEndpoint>();
    services.AddScoped<TelegramUpdateEndpoint, MyTicketsEndpoint>();
    services.AddScoped<TelegramUpdateEndpoint, MangaEndpoint>();
    services.AddScoped<TelegramUpdateEndpoint, DefaultTgUpdateEndpoint>();

    #endregion

    #region Telegram Handlers

    services.AddSingleton<ITelegramHandler, TelegramHandler>();

    #endregion

    #region Database

    // TODO: refactor
    services.AddDbContext<BotDbContext>((provider, opt) =>
    {
        var connection = provider.GetRequiredService<IConfiguration>()
            .GetConnectionString("Database");
        opt.UseNpgsql(connection, x => x.MigrationsAssembly("Nude.Tg.Bot"));
    });
    services.AddDbContextFactory<BotDbContext>((provider, opt) =>
    {
        var connection = provider.GetRequiredService<IConfiguration>()
            .GetConnectionString("Database");
        opt.UseNpgsql(connection, x => x.MigrationsAssembly("Nude.Tg.Bot"));
    });

    #endregion

    #region Background

    services.AddBgService<ConvertBgService>("Converting Background Service");

    #endregion

    #region Services

    services.AddScoped<ITelegraphMangaService, TelegraphMangaService>();
    services.AddScoped<IConvertTicketsService, ConvertTicketsService>();
    services.AddScoped<IMessagesStore, MessageStore>();

    #endregion

}).UseSerilog(Log.Logger);

var host = builder.Build();
await host.StartAsync();

await BotInitializer.StartReceiveAsync(host.Services);
await HttpServer.StartListenAsync(host.Services);