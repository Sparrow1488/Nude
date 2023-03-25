using Nude.Models.Abstractions;

namespace Nude.Models.Tickets.Converting;

public class ConvertingTicket : IAuditable<int>
{
    public int Id { get; set; }
    public long ChatId { get; set; }
    public int ParsingId { get; set; }
    public ConvertingStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}