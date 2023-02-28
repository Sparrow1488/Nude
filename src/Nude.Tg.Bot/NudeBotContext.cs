using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nude.API.Data.Contexts;
using Nude.Tg.Bot.Clients.Nude;
using Nude.Tg.Bot.Clients.Telegraph;
using Nude.Tg.Bot.Constants;
using Nude.Tg.Bot.Endpoints;
using Nude.Tg.Bot.Endpoints.Base;
using Nude.Tg.Bot.Endpoints.Update;
using Nude.Tg.Bot.Handlers;
using Nude.Tg.Bot.Resolvers;
using Serilog;
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
    
    private IHost CreateHost()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .CreateLogger();
        
        var host = Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddScoped<INudeClient, NudeClient>();
                services.AddSingleton<ITelegraphClient, DefaultTelegraphClient>();
                services.AddSingleton(x =>
                {
                    var token = x.GetRequiredService<IConfiguration>()[BotDefaults.TelegramAccessTokenSection];
                    return new TelegramBotClient(token);
                });

                #region Endpoints

                services.AddSingleton<EndpointsResolver>();
                
                services.AddScoped<TelegramUpdateEndpoint, NudeTgEndpoint>();
                services.AddScoped<TelegramUpdateEndpoint, DefaultTgUpdateEndpoint>();

                #endregion

                services.AddSingleton<ITelegramHandler, TelegramHandler>();
                services.AddSingleton(_ => this);

                #region Database

                services.AddDbContext<BotDbContext>((provider, opt) =>
                {
                    var connection = provider.GetRequiredService<IConfiguration>()
                        .GetConnectionString("Database");
                    opt.UseNpgsql(connection, x => x.MigrationsAssembly("Nude.Tg.Bot"));
                });

                #endregion

            }).UseSerilog(Log.Logger);
        
        return host.Build();
    }
}