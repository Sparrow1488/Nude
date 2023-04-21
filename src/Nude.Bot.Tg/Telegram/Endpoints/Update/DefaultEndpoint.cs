using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class DefaultEndpoint : TelegramUpdateEndpoint
{
    // TODO: рандомайзер прикольных слов
    public override async Task HandleAsync() =>
        await BotClient.SendTextMessageAsync(ChatId, "😧Неизвестная команда");

    public override bool CanHandle() => true;
}