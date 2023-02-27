using Telegram.Bot;

namespace Nude.Tg.Bot.Endpoints.Update;

public class TelegramDefaultUpdateEndpoint : TelegramUpdateEndpoint
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