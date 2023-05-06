using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Constants;
using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Blacklist;

public class BlacklistResetEndpoint : TelegramUpdateCommandEndpoint
{
    private readonly INudeClient _client;

    public BlacklistResetEndpoint(INudeClient client) : base(NavigationCommands.ResetBlacklist)
    {
        _client = client;
    }

    public override async Task HandleAsync()
    {
        var authClient = _client.AuthorizeClient(UserSession);
        var result = await authClient.SetDefaultBlacklistAsync();

        if (result.IsSuccess)
        {
            var blacklist = result.ResultValue;
            var tagsMessage = await MessagesStore.GetBlacklistTagsMessageAsync(blacklist.Tags);

            await MessageAsync(tagsMessage);
            return;
        }

        await MessageAsync("Что-то идет не по плану...");
    }
}