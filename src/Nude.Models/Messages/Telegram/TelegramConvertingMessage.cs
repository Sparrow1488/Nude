using Nude.Models.Abstractions;

namespace Nude.Models.Messages.Telegram;

public class TelegramConvertingMessage : IEntity
{
    public int Id { get; set; }
    public int MessageId { get; set; }
    public long ChatId { get; set; }
    public string? Text { get; set; }
    public int ConvertTicketId { get; set; }
    public ParseModeWrapper ParseMode { get; set; }
}