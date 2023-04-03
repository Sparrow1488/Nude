using Microsoft.Extensions.DependencyInjection;
using Nude.Tg.Bot.Telegram.Endpoints.Base;
using Nude.Tg.Bot.Telegram.Endpoints.Update;

namespace Nude.Tg.Bot.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddTelegramEndpoints(this IServiceCollection services)
    {
        services.AddScoped<TelegramUpdateEndpoint, StartEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, MenuEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, MyTicketsEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, MangaEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, RandomEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, FindEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, DefaultTgUpdateEndpoint>();
        
        return services;
    }
}