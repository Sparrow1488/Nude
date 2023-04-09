using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Messages;

public class UserMessages : IEntity
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public long ChatId { get; set; }
    public long MessageId { get; set; }
}