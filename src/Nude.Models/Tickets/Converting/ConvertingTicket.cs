using Nude.Models.Abstractions;

namespace Nude.Models.Tickets.Converting;

public class ConvertingTicket : IEntity
{
    public int Id { get; set; }
    public long ChatId { get; set; }
    public string ParsingTicketId { get; set; }
    // public int MangaId { get; set; }
    // public string MangaUrl { get; set; }
    public ConvertingStatus Status { get; set; }
    // TODO: USER
}