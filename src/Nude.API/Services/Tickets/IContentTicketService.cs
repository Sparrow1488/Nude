using Nude.API.Models.Tickets;

namespace Nude.API.Services.Tickets;

public interface IContentTicketService
{
    Task<ContentTicket> CreateAsync(string contentUrl);
    Task<ContentTicket?> GetByIdAsync(int id);
    Task<ContentTicket?> FindSimilarAsync(string sourceUrl);
    Task<ContentTicket?> GetWaitingAsync();
    Task DeleteAsync(ContentTicket ticket);
}