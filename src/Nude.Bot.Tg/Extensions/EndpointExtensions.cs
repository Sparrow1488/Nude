using Microsoft.Extensions.DependencyInjection;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Nude.Bot.Tg.Telegram.Endpoints.Update;

namespace Nude.Bot.Tg.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddTelegramEndpoints(this IServiceCollection services)
    {
        services.AddScoped<TelegramUpdateEndpoint, KeyboardsEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, PicTagEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, PicturesByTagsEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, StartEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, HelpEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, MangaEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, RandomEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, SourcesEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, DefaultTgUpdateEndpoint>();

        return services;
    }
}