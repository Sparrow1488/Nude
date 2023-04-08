using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Tickets.Contexts;

public class ContentFormatTicketContext : IEntity
{
    public int Id { get; set; }
    public string EntityId { get; set; } = null!;
    public string EntityType { get; set; } = null!;
}