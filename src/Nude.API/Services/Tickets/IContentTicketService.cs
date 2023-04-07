using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.Subscribers;

namespace Nude.API.Services.Tickets;

public interface IContentTicketService
{
    Task<ContentTicket> CreateAsync(string sourceUrl);
    Task<ContentTicket?> GetByIdAsync(int id);
    Task<ContentTicket?> FindSimilarAsync(string sourceUrl);
    Task<Subscriber> SubscribeAsync(ContentTicket ticket, string callback);
}