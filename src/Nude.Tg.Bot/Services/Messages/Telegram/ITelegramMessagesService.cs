using Nude.Models.Messages.Telegram;

namespace Nude.Tg.Bot.Services.Messages.Telegram;

public interface ITelegramMessagesService
{
    Task<TelegramConvertingMessage?> GetByTicketIdAsync(int convertTicketId);
    Task<IEnumerable<TelegramConvertingMessage>> GetSimilarByTicketIdAsync(int convertTicketId);
    Task<TelegramConvertingMessage> CreateMessageAsync(TelegramConvertingMessage message);
    Task DeleteMessageAsync(TelegramConvertingMessage message);
}