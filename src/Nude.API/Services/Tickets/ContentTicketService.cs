using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.Contexts;
using Nude.API.Models.Tickets.States;
using Nude.API.Models.Tickets.Subscribers;
using Nude.Data.Infrastructure.Contexts;

namespace Nude.API.Services.Tickets;

// ReSharper disable once InvertIf

public class ContentTicketService : IContentTicketService
{
    private readonly FixedAppDbContext _context;

    public ContentTicketService(FixedAppDbContext context)
    {
        _context = context;
    }
    
    public async Task<ContentTicket> CreateAsync(string sourceUrl)
    {
        var request = new ContentTicket
        {
            Status = ReceiveStatus.WaitToProcess,
            Context = new TicketContext
            {
                ContentUrl = sourceUrl
            }
        };

        await _context.AddAsync(request);
        await _context.SaveChangesAsync();

        return request;
    }

    public Task<ContentTicket?> GetByIdAsync(int id)
    {
        return _context.ContentRequests
            .AsQueryable()
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<ContentTicket?> FindSimilarAsync(string sourceUrl)
    {
        return _context.ContentRequests
            .AsQueryable()
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.Context.ContentUrl.Contains(sourceUrl));
    }

    public async Task<Subscriber> SubscribeAsync(ContentTicket ticket, string callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        
        var alreadySubscribed = ticket.Subscribers.Any(x => x.CallbackUrl == callback);

        if (!alreadySubscribed)
        {
            var subscriber = new Subscriber
            {
                CallbackUrl = callback,
                ContentTicketId = ticket.Id,
                NotifyStatus = NotifyStatus.All
            };
            
            await _context.AddAsync(subscriber);
            await _context.SaveChangesAsync();
            
            ticket.Subscribers.Add(subscriber);
        }
        
        return ticket.Subscribers.First(x => x.CallbackUrl == callback);
    }
}

public static class ReceiveContentRequestExtensions
{
    public static IQueryable<ContentTicket> IncludeDependencies(
        this IQueryable<ContentTicket> queryable)
    {
        return queryable
            .Include(x => x.Result)
            .Include(x => x.Context)
            .Include(x => x.Subscribers);
    }
}