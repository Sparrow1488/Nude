using Nude.Bot.Tg.Services.Manga;
using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class RandomEndpoint : TelegramUpdateCommandEndpoint
{
    private readonly ITelegraphMangaService _tghMangaService;

    public RandomEndpoint(ITelegraphMangaService tghMangaService) : base("/random")
    {
        _tghMangaService = tghMangaService;
    }
    
    public override async Task HandleAsync()
    {
        var manga = await _tghMangaService.GetRandomAsync();
        await MessageAsync(await MessagesStore.GetTghMessageAsync(manga!));
    }
}