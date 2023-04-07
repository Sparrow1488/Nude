using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.Contexts;
using Nude.API.Models.Tickets.States;
using Nude.Data.Infrastructure.Contexts;

namespace Nude.API.Services.Tickets;

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
            Context = new ReceiveContext
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
        return _context.ReceiveRequests
            .AsQueryable()
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.Id == id);
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