using Nude.Models.Abstractions;

namespace Nude.Models.Tickets.Parsing;

public sealed class ParsingResult : IEntity
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string? EntityId { get; set; }
    public string StatusCode { get; set; }
    public ParsingTicket Ticket { get; set; }
    public int TicketId { get; set; }
}