using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Messages;

public class UserMessages : IEntity
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public long ChatId { get; set; }
    public int TicketId { get; set; }

    public string UserKey { get; set; } = null!;
    public string TicketType { get; set; } = null!;
    public long MessageId { get; set; }
}