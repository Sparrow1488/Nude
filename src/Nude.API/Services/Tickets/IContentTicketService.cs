using Nude.API.Models.Tickets;
using Nude.API.Services.Tickets.Results;

namespace Nude.API.Services.Tickets;

public interface IContentTicketService
{
    Task<ContentTicketCreationResult> CreateAsync(string contentUrl);
    Task<ContentTicket?> GetByIdAsync(int id);
    Task<ICollection<ContentTicket>> GetSimilarWaitingAsync();
    Task DeleteRangeAsync(IEnumerable<ContentTicket> tickets);
}