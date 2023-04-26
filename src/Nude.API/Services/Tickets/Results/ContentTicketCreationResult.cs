using Nude.API.Infrastructure.Abstractions;
using Nude.API.Models.Tickets;

namespace Nude.API.Services.Tickets.Results;

public class ContentTicketCreationResult : IServiceResult<ContentTicket>
{
    public bool IsSuccess { get; set; }
    public Exception? Exception { get; set; }
    public ContentTicket? Result { get; set; }
}