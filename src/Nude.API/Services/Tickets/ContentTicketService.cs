using Microsoft.EntityFrameworkCore;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Infrastructure.Utility;
using Nude.API.Models.Tickets;
using Nude.API.Services.Tickets.Results;
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
    
    public async Task<ContentTicketCreationResult> CreateAsync(string contentUrl)
    {
        var entryType = ContentAware.DetectEntryTypeByUrl(contentUrl);

        if (!string.IsNullOrWhiteSpace(entryType))
        {
            var request = new ContentTicket
            {
                ContentKey = ContentKeyGenerator.Generate(entryType, contentUrl),
                ContentUrl = contentUrl
            };

            await _context.AddAsync(request);
            await _context.SaveChangesAsync();
            return new ContentTicketCreationResult { IsSuccess = true, Result = request };
        }

        var exception = new SourceNotAvailableException(contentUrl);
        return new ContentTicketCreationResult { IsSuccess = false, Exception = exception };
    }

    public Task<ContentTicket?> GetByIdAsync(int id)
    {
        return _context.ContentTickets
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<ContentTicket>> GetSimilarWaitingAsync()
    {
        var ticket = await _context.ContentTickets
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();
        
        if (ticket != null)
        {
            return await _context.ContentTickets
                .Where(x => x.ContentKey == ticket.ContentKey)
                .ToListAsync();
        }

        return new List<ContentTicket>();
    }

    public async Task DeleteRangeAsync(IEnumerable<ContentTicket> tickets)
    {
        _context.RemoveRange(tickets);
        await _context.SaveChangesAsync();
    }
}