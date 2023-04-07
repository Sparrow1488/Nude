using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Tickets.Contexts;

public class TicketContext : IEntity
{
    public int Id { get; set; }
    public string ContentUrl { get; set; }
    public string? ContentId { get; set; }
}
