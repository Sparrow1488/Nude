using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Requests.Contexts;

public class ReceiveContext : IEntity
{
    public int Id { get; set; }
    public string ContentUrl { get; set; }
    public string? ContentId { get; set; }
}
