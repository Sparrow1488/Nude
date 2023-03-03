using Microsoft.Extensions.DependencyInjection;
using Nude.Tg.Bot.Telegram.Endpoints.Base;
using Nude.Tg.Bot.Telegram.Endpoints.Update;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Tg.Bot.Services.Resolvers;

public class EndpointsResolver
{
    private readonly IServiceProvider _services;

    public EndpointsResolver(IServiceProvider services)
    {
        _services = services;
    }

    public TelegramUpdateEndpoint GetUpdateHandler(Update update, ITelegramBotClient botClient)
    {
        var endpoints = _services.GetServices<TelegramUpdateEndpoint>().ToList();
        endpoints.ForEach(x =>
        {
            x.Update = update;
            x.BotClient = botClient;
        });
        
        var handler = endpoints.FirstOrDefault(x => x.CanHandle());
        if (handler != null)
            return handler;
        
        return ActivatorUtilities.CreateInstance<DefaultTgUpdateEndpoint>(_services);
    }
}