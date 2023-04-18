using Nude.API.Models.Abstractions;
using Nude.API.Models.Messages.Details;
using Nude.API.Models.Users;

namespace Nude.API.Models.Messages;

public class UserMessage : IEntity
{
    public int Id { get; set; }
    public long ChatId { get; set; }
    public long MessageId { get; set; }
    public MessageDetails Details { get; set; } = null!;
    
    public TelegramUser Owner { get; set; } = null!;
    public int OwnerId { get; set; }
}