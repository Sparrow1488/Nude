using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Messages;

public class UserMessages : IEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ChatId { get; set; }
    public int MessageId;
}