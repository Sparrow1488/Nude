using Nude.API.Contracts.Formats.Responses;
using Nude.API.Models.Formats;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class RandomEndpoint : TelegramUpdateCommandEndpoint
{
    private readonly INudeClient _client;

    public RandomEndpoint(INudeClient client) : base("/random")
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