using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Tickets;

public class ContentTicket : IEntity
{
    public int Id { get; set; }
    public string ContentKey { get; set; } = null!;
    public string ContentUrl { get; set; } = null!;
}