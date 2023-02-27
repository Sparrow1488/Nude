using Microsoft.Extensions.DependencyInjection;
using Nude.Tg.Bot.Endpoints.Base;
using Nude.Tg.Bot.Endpoints.Update;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Tg.Bot.Resolvers;

public class EndpointsResolver
{
    private readonly IServiceProvider _services;
    private readonly NudeBotContext _botContext;

    public EndpointsResolver(IServiceProvider services, NudeBotContext botContext)
    {
        _services = services;
        _botContext = botContext;
    }

    public TelegramUpdateEndpoint GetUpdateHandler(Update update, ITelegramBotClient botClient)
    {
        var endpoints = _services.GetServices<TelegramUpdateEndpoint>().ToList();
        endpoints.ForEach(x =>
        {
            x.Update = update;
            x.BotClient = botClient;
            x.Context = _botContext;
        });
        
        var handler = endpoints.FirstOrDefault(x => x.CanHandle());
        if (handler != null)
            return handler;
        
        return ActivatorUtilities.CreateInstance<DefaultTgUpdateEndpoint>(_services);
    }
}