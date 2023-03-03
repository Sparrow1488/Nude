using Nude.Models.Abstractions;

namespace Nude.Models.Tickets.Parsing;

public sealed class ParsingMeta : IEntity
{
    public int Id { get; set; }
    public string? SourceItemId { get; set; }
    public string SourceUrl { get; set; }
    public ParsingEntityType EntityType { get; set; }
    public ParsingTicket Ticket { get; set; }
    public int TicketId { get; set; }
}