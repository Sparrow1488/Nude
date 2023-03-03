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
using Nude.Tg.Bot.Services.Background;
using Nude.Tg.Bot.Services.Resolvers;
using Nude.Tg.Bot.Telegram.Endpoints.Base;
using Nude.Tg.Bot.Telegram.Endpoints.Update;
using Nude.Tg.Bot.Telegram.Handlers;
using Serilog;
using Serilog.Events;
using Telegram.Bot;

namespace Nude.Tg.Bot;

public class NudeBotContext
{
    private readonly IHost _host;

    private NudeBotContext()
    {
        _host = CreateHost();
    }

    public IServiceProvider Services => _host.Services;

    public static NudeBotContext CreateDefault() => new();

    public IHost GetHost() => _host;
    
    private IHost CreateHost()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .CreateLogger();
        
        var builder = Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddScoped<INudeClient, NudeClient>();
                services.AddSingleton<ITelegraphClient, DefaultTelegraphClient>();
                services.AddSingleton<ITelegramBotClient>(x =>
                {
                    var token = x.GetRequiredService<IConfiguration>()[BotDefaults.TelegramAccessTokenSection];
                    return new TelegramBotClient(token);
                });

                #region Http Routes

                services.AddScoped<CallbackRoute>();

                #endregion
                
                #region Endpoints

                services.AddSingleton<EndpointsResolver>();
                
                services.AddScoped<TelegramUpdateEndpoint, NudeTgEndpoint>();
                services.AddScoped<TelegramUpdateEndpoint, DefaultTgUpdateEndpoint>();

                #endregion

                #region Telegram Handlers

                services.AddSingleton<ITelegramHandler, TelegramHandler>();
                services.AddSingleton(_ => this);

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

            }).UseSerilog(Log.Logger);

        return builder.Build();
    }
}