using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Tickets.Subscribers;
using Nude.Data.Infrastructure.Contexts;

namespace Nude.API.Services.Subscribers;

public class SubscribersService : ISubscribersService
{
    private readonly FixedAppDbContext _context;

    public SubscribersService(FixedAppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Subscriber> CreateAsync(string entityId, string entityType, string callbackUrl)
    {
        var subscriber = new Subscriber
        {
            EntityId = entityId,
            EntityType = entityType,
            CallbackUrl = callbackUrl
        };

        await _context.AddAsync(subscriber);
        await _context.SaveChangesAsync();

        return subscriber;
    }

    public async Task<IEnumerable<Subscriber>> FindAsync(string entityId, string entityType)
    {
        var subscribers = await _context.Subscribers
            .Where(x => x.EntityId == entityId && x.EntityType == entityType)
            .ToListAsync();

        return subscribers;
    }

    public async Task DeleteAsync(Subscriber subscriber)
    {
        var exists = await _context.Subscribers.AnyAsync(x => x.Id == subscriber.Id);
        if (!exists)
        {
            _context.Remove(subscriber);
            await _context.SaveChangesAsync();
        }
    }
}