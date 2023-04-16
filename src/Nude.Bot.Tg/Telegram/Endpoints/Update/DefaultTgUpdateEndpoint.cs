using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class DefaultTgUpdateEndpoint : TelegramUpdateEndpoint
{
    // TODO: рандомайзер прикольных слов
    public override async Task HandleAsync(Message message) =>
        await BotClient.SendTextMessageAsync(ChatId, "Неизвестная команда");

    public override bool CanHandle() => true;
}