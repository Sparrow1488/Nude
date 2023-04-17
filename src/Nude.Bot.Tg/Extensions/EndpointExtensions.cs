using Microsoft.Extensions.DependencyInjection;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Nude.Bot.Tg.Telegram.Endpoints.Update;
using Nude.Bot.Tg.Telegram.Endpoints.Update.Auxiliary;
using Nude.Bot.Tg.Telegram.Endpoints.Update.Manga;
using Nude.Bot.Tg.Telegram.Endpoints.Update.Pictures;

namespace Nude.Bot.Tg.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddTelegramEndpoints(this IServiceCollection services)
    {
        services.AddScoped<TelegramUpdateEndpoint, KeyboardsEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, PictagEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, PicturesByTagsEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, StartEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, HelpEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, MangaEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, RandomMangaEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, SourcesEndpoint>();
        services.AddScoped<TelegramUpdateEndpoint, DefaultEndpoint>();

        return services;
    }
}