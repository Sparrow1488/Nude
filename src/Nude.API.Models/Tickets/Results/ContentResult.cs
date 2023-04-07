using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Tickets.Results;

public class ContentResult : IEntity
{
    public int Id { get; set; }
    public string EntityId { get; set; } = null!;
    public string Code { get; set; } = null!;
}