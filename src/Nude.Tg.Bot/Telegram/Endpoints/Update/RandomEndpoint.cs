using Nude.Tg.Bot.Services.Manga;
using Nude.Tg.Bot.Services.Messages.Store;
using Nude.Tg.Bot.Telegram.Endpoints.Base;

namespace Nude.Tg.Bot.Telegram.Endpoints.Update;

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