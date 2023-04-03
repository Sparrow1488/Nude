using Nude.Tg.Bot.Telegram.Endpoints.Base;
using Telegram.Bot;

namespace Nude.Tg.Bot.Telegram.Endpoints.Update;

public class DefaultTgUpdateEndpoint : TelegramUpdateEndpoint
{
    // TODO: рандомайзер прикольных слов
    public override async Task HandleAsync() =>
        await BotClient.SendTextMessageAsync(ChatId, "Неизвестная команда");

    public override bool CanHandle() => true;
}