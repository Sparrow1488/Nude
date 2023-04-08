using Nude.API.Models.Tickets.Subscribers;

namespace Nude.API.Services.Subscribers;

public interface ISubscribersService
{
    Task<Subscriber> CreateAsync(string entityId, string entityType, string callbackUrl);
    Task<IEnumerable<Subscriber>> FindAsync(string entityId, string entityType);
}