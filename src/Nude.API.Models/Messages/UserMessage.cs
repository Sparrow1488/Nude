using Nude.API.Models.Abstractions;
using Nude.API.Models.Messages.Details;
using Nude.API.Models.Users;

namespace Nude.API.Models.Messages;

public class UserMessage : IEntity
{
    public int Id { get; set; }
    public long ChatId { get; set; }
    public int MessageId { get; set; }
    public MessageDetails Details { get; set; } = null!;
    public int DetailsId { get; set; }
    
    public TelegramUser Owner { get; set; } = null!;
    public int OwnerId { get; set; }
}