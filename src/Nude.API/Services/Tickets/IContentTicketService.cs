using Nude.API.Models.Tickets;
using Nude.API.Models.Users;
using Nude.API.Services.Tickets.Results;

namespace Nude.API.Services.Tickets;

public interface IContentTicketService
{
    Task<ContentTicketCreationResult> CreateAsync(string contentUrl, User owner);
    Task<ContentTicket?> GetByIdAsync(int id);
    Task<ICollection<ContentTicket>> GetUserTicketsAsync(int userId);
    Task<ICollection<ContentTicket>> GetSimilarWaitingAsync();
    Task DeleteRangeAsync(IEnumerable<ContentTicket> tickets);
}