using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.States;

namespace Nude.API.Services.Tickets;

public interface IContentTicketService
{
    Task<ContentTicket> CreateAsync(string sourceUrl);
    Task<ContentTicket> UpdateStatusAsync(ContentTicket ticket, ReceiveStatus status);
    Task<ContentTicket> UpdateResultAsync(ContentTicket ticket, string entityId, string code);
    Task<ContentTicket?> GetByIdAsync(int id);
    Task<ContentTicket?> FindSimilarAsync(string sourceUrl);
    Task<ContentTicket?> GetWaitingAsync();
    // Task<Subscriber> SubscribeAsync(ContentTicket ticket, string callback);
}