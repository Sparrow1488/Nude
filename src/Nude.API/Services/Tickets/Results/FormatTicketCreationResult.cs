using Nude.API.Models.Tickets;

namespace Nude.API.Services.Tickets.Results;

public class FormatTicketCreationResult
{
    public bool IsSuccess { get; set; }
    public ContentFormatTicket? FormatTicket { get; set; }
    public Exception? Exception { get; set; }
}