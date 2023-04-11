using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Messages;

public class UserMessage : IEntity
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public long ChatId { get; set; }
    public int TicketId { get; set; }

    public string ContentKey { get; set; } = null!;
    public long MessageId { get; set; }
}