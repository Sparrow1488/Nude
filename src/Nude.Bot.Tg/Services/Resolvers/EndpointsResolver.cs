using Microsoft.Extensions.DependencyInjection;
using Nude.Bot.Tg.Services.Users;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Nude.Bot.Tg.Telegram.Endpoints.Update;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Bot.Tg.Services.Resolvers;

public class EndpointsResolver
{
    private readonly IServiceProvider _services;

    public EndpointsResolver(IServiceProvider services)
    {
        _services = services;
    }

    public TelegramUpdateEndpoint GetUpdateHandler(
        Update update, 
        ITelegramBotClient botClient,
        UserSession session)
    {
        var endpoints = _services.GetServices<TelegramUpdateEndpoint>().ToList();
        endpoints.ForEach(x =>
        {
            x.Update = update;
            x.BotClient = botClient;
            x.ServiceProvider = _services;
            x.UserSession = session;
        });
        
        var handler = endpoints.FirstOrDefault(x => x.CanHandle());
        if (handler != null)
            return handler;
        
        return ActivatorUtilities.CreateInstance<DefaultTgUpdateEndpoint>(_services);
    }
}