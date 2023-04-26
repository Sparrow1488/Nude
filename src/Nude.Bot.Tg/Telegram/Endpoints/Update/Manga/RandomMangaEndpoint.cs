using Nude.API.Contracts.Formats.Responses;
using Nude.API.Models.Formats;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Constants;
using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Manga;

public class RandomMangaEndpoint : TelegramUpdateCommandEndpoint
{
    private readonly INudeClient _client;

    public RandomMangaEndpoint(INudeClient client) : base(NavigationCommands.RandomManga)
    {
        _client = client;
    }
    
    public override async Task HandleAsync()
    {
        var mangaResult = await _client.GetRandomMangaAsync(FormatType.Telegraph);
        if (mangaResult.IsSuccess)
        {
            var telegraph = (TelegraphFormatResponse) mangaResult.ResultValue.Formats.First(
                x => x is TelegraphFormatResponse
            );
            
            await MessageAsync(await MessagesStore.GetReadMangaMessageAsync(telegraph.Url));
            return;
        }

        await MessageAsync("Увы, не найдено ни одной манги!");
    }
}