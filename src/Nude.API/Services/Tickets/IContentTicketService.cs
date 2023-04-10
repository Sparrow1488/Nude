using Nude.API.Models.Tickets;

namespace Nude.API.Services.Tickets;

public interface IContentTicketService
{
    Task<ContentTicket> CreateAsync(string contentUrl);
    Task<ContentTicket?> GetByIdAsync(int id);
    Task<ContentTicket?> FindSimilarAsync(string sourceUrl);
    Task<ICollection<ContentTicket>> GetSimilarWaitingAsync();
    Task DeleteAsync(ContentTicket ticket);
    Task DeleteRangeAsync(IEnumerable<ContentTicket> tickets);
}