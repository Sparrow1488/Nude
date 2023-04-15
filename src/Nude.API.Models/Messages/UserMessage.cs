using Nude.API.Models.Abstractions;
using Nude.API.Models.Users;

namespace Nude.API.Models.Messages;

public class UserMessage : IEntity
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public string ContentKey { get; set; } = null!;
    public long MessageId { get; set; }
    public long ChatId { get; set; }
    public TelegramUser Owner { get; set; } = null!;
    public int OwnerId { get; set; }
}