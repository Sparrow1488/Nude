using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nude.Tg.Bot.Clients.Telegraph;
using Nude.Tg.Bot.Constants;
using Nude.Tg.Bot.Endpoints;
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
                services.AddSingleton<ITelegraphClient, DefaultTelegraphClient>();
                services.AddSingleton(x =>
                {
                    var token = x.GetRequiredService<IConfiguration>()[BotDefaults.TelegramAccessTokenSection];
                    return new TelegramBotClient(token);
                });

                #region Endpoints

                services.AddSingleton<EndpointsResolver>();
                
                services.AddSingleton<TelegramUpdateEndpoint, NudeTelegramEndpoint>();
                services.AddSingleton<TelegramUpdateEndpoint, TelegramDefaultUpdateEndpoint>();

                #endregion

                services.AddSingleton<ITelegramHandler, TelegramHandler>();
                services.AddSingleton(_ => this);

            }).UseSerilog(Log.Logger);
        
        return host.Build();
    }
}