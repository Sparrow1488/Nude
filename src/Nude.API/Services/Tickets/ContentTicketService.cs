using Microsoft.EntityFrameworkCore;
using Nude.API.Infrastructure.Utility;
using Nude.API.Models.Tickets;
using Nude.Data.Infrastructure.Contexts;
using Nude.Data.Infrastructure.Extensions;

namespace Nude.API.Services.Tickets;

#region Rider annotations

// ReSharper disable once InvertIf

#endregion

public class ContentTicketService : IContentTicketService
{
    private readonly AppDbContext _context;

    public ContentTicketService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<ContentTicket> CreateAsync(string contentUrl)
    {
        var entryType = EntryTypeDetector.ByContentUrl(contentUrl);
        
        var request = new ContentTicket
        {
            ContentKey = ContentKeyHelper.CreateContentKey(entryType, contentUrl),
            ContentUrl = contentUrl
        };

        await _context.AddAsync(request);
        await _context.SaveChangesAsync();

        return request;
    }

    public Task<ContentTicket?> GetByIdAsync(int id)
    {
        return _context.ContentTickets
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<ContentTicket?> FindSimilarAsync(string sourceUrl)
    {
        return _context.ContentTickets
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.ContentUrl.Contains(sourceUrl));
    }

    public async Task<ICollection<ContentTicket>> GetSimilarWaitingAsync()
    {
        var ticket = await _context.ContentTickets.FirstOrDefaultAsync();
        if (ticket != null)
        {
            return await _context.ContentTickets
                .Where(x => x.ContentKey == ticket.ContentKey)
                .ToListAsync();
        }

        return new List<ContentTicket>();
    }

    public async Task DeleteAsync(ContentTicket ticket)
    {
        _context.Remove(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<ContentTicket> tickets)
    {
        _context.RemoveRange(tickets);
        await _context.SaveChangesAsync();
    }
}