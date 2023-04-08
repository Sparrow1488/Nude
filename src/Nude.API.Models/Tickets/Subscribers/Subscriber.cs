using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Tickets.Subscribers;

public class Subscriber : IEntity
{
    public int Id { get; set; }
    public string EntityId { get; set; } = null!;
    public string EntityType { get; set; } = null!;
    public string? CallbackUrl { get; set; }
}
