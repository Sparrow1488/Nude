using Nude.Tg.Bot.Services.Manga;
using Nude.Tg.Bot.Services.Messages;
using Nude.Tg.Bot.Services.Messages.Store;
using Nude.Tg.Bot.Telegram.Endpoints.Base;

namespace Nude.Tg.Bot.Telegram.Endpoints.Update;

public class RandomEndpoint : TelegramUpdateEndpoint
{
    private readonly IMessagesStore _messages;
    private readonly ITelegraphMangaService _tghMangaService;

    public RandomEndpoint(
        IMessagesStore messages,
        ITelegraphMangaService tghMangaService)
    {
        _messages = messages;
        _tghMangaService = tghMangaService;
    }
    
    public override async Task HandleAsync()
    {
        var manga = await _tghMangaService.GetRandomAsync();
        if (manga is not null)
        {
            await MessageAsync(await _messages.GetTghMessageAsync(manga));
        }
        else
        {
            await MessageAsync("Ничего не найдено!");
        }
    }

    public override bool CanHandle()
    {
        return MessageText == "/random";
    }
}