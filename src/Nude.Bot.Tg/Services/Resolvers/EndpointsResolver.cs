using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Nude.Bot.Tg.Attributes;
using Nude.Bot.Tg.Services.Users;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Nude.Bot.Tg.Telegram.Endpoints.Update;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Bot.Tg.Services.Resolvers;

public class EndpointsResolver
{
    private static IEnumerable<Type>? _endpoints;

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
        var endpoints = GetEndpoints();
        var implEndpoints = endpoints.Select(CreateEndpoint).ToList();
        implEndpoints.Add(CreateEndpoint(GetDefaultEndpoint()));
        
        implEndpoints.ToList().ForEach(x =>
        {
            x.Update = update;
            x.BotClient = botClient;
            x.ServiceProvider = _services;
            x.UserSession = session;
        });
        
        var handler = implEndpoints.FirstOrDefault(x => x.CanHandle());
        if (handler != null)
            return handler;

        throw new InvalidOperationException("No any endpoint registered");
    }

    private static IEnumerable<Type> GetEndpoints()
    {
        if (_endpoints is null)
        {
            var baseType = typeof(TelegramEndpoint);
            var defaultEndpoint = GetDefaultEndpoint();
        
            var types = Assembly.GetAssembly(typeof(TelegramUpdateEndpoint))!
                .GetTypes()
                .Where(x => 
                    x.IsAssignableTo(baseType) && 
                    x is {IsAbstract: false, IsInterface: false} &&
                    x.GetCustomAttribute<IgnoreEndpointAttribute>() is null)
                .Where(x => x != defaultEndpoint);

            _endpoints = types;
        }

        return _endpoints;
    }

    private static Type GetDefaultEndpoint() => typeof(DefaultEndpoint);

    private TelegramUpdateEndpoint CreateEndpoint(Type type)
    {
        return (TelegramUpdateEndpoint) ActivatorUtilities.CreateInstance(_services, type);
    }
}