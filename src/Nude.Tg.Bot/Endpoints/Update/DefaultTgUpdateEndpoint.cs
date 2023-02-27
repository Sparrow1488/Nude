using Nude.Tg.Bot.Endpoints.Base;
using Telegram.Bot;

namespace Nude.Tg.Bot.Endpoints.Update;

public class DefaultTgUpdateEndpoint : TelegramUpdateEndpoint
{
    public override async Task HandleAsync()
    {
        // TODO: рандомайзер прикольных слов
        await BotClient.SendTextMessageAsync(ChatId, "Неизвестная команда");
    }

    public override bool CanHandle()
    {
        return true;
    }
}