using Nude.API.Models.Tickets;

namespace Nude.API.Services.Tickets;

public interface IContentTicketService
{
    Task<ContentTicket> CreateAsync(string sourceUrl);
    Task<ContentTicket?> GetByIdAsync(int id);
}