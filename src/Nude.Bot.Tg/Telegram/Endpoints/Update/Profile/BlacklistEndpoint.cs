using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Constants;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Profile;

public class BlacklistEndpoint : TelegramUpdateCommandEndpoint
{
    private readonly INudeClient _client;
    private readonly IMessagesStore _messagesStore;

    public BlacklistEndpoint(
        INudeClient client,
        IMessagesStore messagesStore) : base(NavigationCommands.Blacklist)
    {
        _client = client;
        _messagesStore = messagesStore;
    }

    public override async Task HandleAsync()
    {
        var authClient = _client.AuthorizeClient(UserSession);
        var blacklistResult = await authClient.GetBlacklistAsync();

        if (!blacklistResult.IsSuccess)
        {
            await authClient.SetDefaultBlacklistAsync();
            blacklistResult = await authClient.GetBlacklistAsync();
        }

        var message = await _messagesStore.GetBlacklistTagsMessageAsync(blacklistResult.ResultValue);
        await MessageAsync(message);
    }
}