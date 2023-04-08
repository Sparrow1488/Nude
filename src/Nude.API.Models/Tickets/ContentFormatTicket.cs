using Nude.API.Models.Abstractions;
using Nude.API.Models.Formats;
using Nude.API.Models.Tickets.Contexts;
using Nude.API.Models.Tickets.States;

namespace Nude.API.Models.Tickets;

public class ContentFormatTicket : IEntity
{
    public int Id { get; set; }
    public FormatType FormatType { get; set; }
    public FormattingStatus Status { get; set; }
    public FormattedContent? Result { get; set; } = null!;
    public ContentFormatTicketContext Context { get; set; } = null!;
}