using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Messages.Details;

public abstract class MessageDetails : IEntity
{
    public int Id { get; set; }
    public UserMessage Message { get; set; } = null!;
}