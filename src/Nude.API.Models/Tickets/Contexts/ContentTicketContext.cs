using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Tickets.Contexts;

public class ContentTicketContext : IEntity
{
    public int Id { get; set; }
    public string ContentUrl { get; set; }
    public string? ContentId { get; set; }
}