using Nude.Models.Abstractions;

namespace Nude.Models.Tickets;

public sealed class FeedBackInfo : IEntity
{
    public int Id { get; set; }
    public string? CallbackUrl { get; set; }
}