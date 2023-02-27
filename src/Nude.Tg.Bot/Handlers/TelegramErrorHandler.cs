using Telegram.Bot;

namespace Nude.Tg.Bot.Handlers;

public static class TelegramErrorHandler
{
    public static Task HandleErrorAsync(
        ITelegramBotClient botClient, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        throw exception;
    }
}