using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Bot.Tg.Telegram.Handlers;

public interface ITelegramHandler
{
    Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ctk);
    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ctk);
}