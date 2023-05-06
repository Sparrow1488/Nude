using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Constants;
using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Blacklist;

public class BlacklistCleanEndpoint : TelegramUpdateCommandEndpoint
{
    private readonly INudeClient _client;

    public BlacklistCleanEndpoint(INudeClient client) : base(NavigationCommands.CleanBlacklist)
    {
        _client = client;
    }

    public override async Task HandleAsync()
    {
        var authClient = _client.AuthorizeClient(UserSession);
        var defaultTags = (await authClient.SetDefaultBlacklistAsync()).ResultValue.Tags;

        await authClient.DeleteBlacklistTagsAsync(defaultTags.Select(x => x.Value).ToArray());

        var userBlacklist = (await authClient.GetBlacklistAsync()).ResultValue;
        var tagsMessage = await MessagesStore.GetBlacklistTagsMessageAsync(userBlacklist.Tags);

        await MessageAsync(tagsMessage);
    }
}