using Nude.API.Models.Formats;
using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.States;
using Nude.API.Services.Tickets.Results;

namespace Nude.API.Services.Tickets;

public interface IContentFormatTicketService
{
    Task<FormatTicketCreationResult> CreateAsync(string entityId, string entityType, FormatType type);
    Task<ContentFormatTicket?> GetByIdAsync(int id);
    Task<ContentFormatTicket?> GetWaitingAsync();
    Task<ContentFormatTicket> UpdateStatusAsync(ContentFormatTicket ticket, FormattingStatus status);
    Task<ContentFormatTicket> UpdateResultAsync(ContentFormatTicket ticket, FormattedContent result);
}