using Nude.Tg.Bot.Services.Manga;
using Nude.Tg.Bot.Telegram.Endpoints.Base;

namespace Nude.Tg.Bot.Telegram.Endpoints.Update;

public class RandomEndpoint : TelegramUpdateEndpoint
{
    private readonly ITelegraphMangaService _tghMangaService;

    public RandomEndpoint(ITelegraphMangaService tghMangaService)
    {
        _tghMangaService = tghMangaService;
    }
    
    public override async Task HandleAsync()
    {
        var manga = await _tghMangaService.GetRandomAsync();
        await MessageAsync(manga?.TghUrl ?? "Ничего не найдено!");
    }

    public override bool CanHandle()
    {
        return MessageText == "/random";
    }
}