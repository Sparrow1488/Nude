using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Services.Utils;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Profile;

public class BlacklistDeleteEndpoint : TelegramUpdateCommandEndpoint
{
    private const string Command = "/bl del";
    private readonly INudeClient _client;
    
    public BlacklistDeleteEndpoint(INudeClient client) : base(Command)
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

        var result = await authClient.DeleteBlacklistTagsAsync(inputTags);
        if (result.IsSuccess)
        {
            var message = "Теги удалены: " + string.Join(
                " ",
                result.ResultValue.Select(x => $"`{x.Value}`")
            );
            await MessageAsync(message, ParseMode.MarkdownV2);
            return;
        }

        await MessageAsync("Не удалось удалить теги...");
    }
}