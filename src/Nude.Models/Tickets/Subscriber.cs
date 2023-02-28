using Nude.Models.Abstractions;

namespace Nude.Models.Tickets;

public sealed class Subscriber : IEntity
{
    public int Id { get; set; }
    public NotifyStatus NotifyStatus { get; set; }
    public FeedBackInfo FeedBackInfo { get; set; }
    public ICollection<ParsingTicket> Tickets { get; set; }
    // TODO: User attach
}