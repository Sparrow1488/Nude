using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Services.Utils;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Blacklist;

public class BlacklistUpdateEndpoint : TelegramUpdateCommandEndpoint
{
    private const string Command = "/bl add";
    private readonly INudeClient _client;

    public BlacklistUpdateEndpoint(INudeClient client) : base(Command)
    {
        _client = client;
    }

    public override async Task HandleAsync()
    {
        var authClient = _client.AuthorizeClient(UserSession);
        var input = MessageText.Replace(Command, "").Trim();
        var inputTags = InputUtils.SplitInputByComma(input);

        if (!inputTags.Any())
        {
            await MessageAsync("Перечислите какие-нибудь теги");
            return;
        }
        
        var tagsResult = await authClient.SetBlacklistTagsAsync(inputTags);
        if (tagsResult.IsSuccess)
        {
            var message = "Добавлены теги\\: " + string.Join(
                ", ", 
                tagsResult.ResultValue.Select(x => $"`{x.Value}`")
            );
            
            var blacklist = (await authClient.GetBlacklistAsync()).ResultValue;
            var tagsMessage = await MessagesStore.GetBlacklistTagsMessageAsync(blacklist.Tags);
            
            await MessageAsync(message + "\n\n" + tagsMessage.Text, ParseMode.MarkdownV2);
            return;
        }

        await MessageAsync("Че-то не получается добавить теги...");
    }
}