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
        var authClient = _client.AuthorizeClient(UserSession);
        var mangaResult = await authClient.GetRandomMangaAsync(FormatType.Telegraph);
        if (mangaResult.IsSuccess)
        {
            var telegraph = mangaResult.ResultValue.Formats.OfType<TelegraphFormatResponse>().First();
            
            await MessageAsync(await MessagesStore.GetReadMangaMessageAsync(telegraph.Url));
            return;
        }

        await MessageAsync("Увы, не найдено ни одной манги!");
    }
}