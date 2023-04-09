using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Formats;
using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.Contexts;
using Nude.API.Models.Tickets.States;
using Nude.API.Services.Tickets.Results;
using Nude.Data.Infrastructure.Contexts;
using Nude.Data.Infrastructure.Extensions;

namespace Nude.API.Services.Tickets;

public class ContentFormatTicketService : IContentFormatTicketService
{
    private readonly AppDbContext _context;

    public ContentFormatTicketService(
        AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<FormatTicketCreationResult> CreateAsync(string entityId, string entityType, FormatType type)
    {
        // TODO: check type supported and entry id exists
        
        var ticket = new ContentFormatTicket
        {
            FormatType = type,
            Context = new ContentFormatTicketContext
            {
                EntityId = entityId,
                EntityType = entityType
            },
            Status = FormattingStatus.WaitToProcess
        };

        await _context.AddAsync(ticket);
        await _context.SaveChangesAsync();

        return new FormatTicketCreationResult
        {
            IsSuccess = true,
            FormatTicket = ticket
        };
    }

    public Task<ContentFormatTicket?> GetByIdAsync(int id)
    {
        return _context.FormatTickets
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<ContentFormatTicket?> GetWaitingAsync()
    {
        return _context.FormatTickets
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.Status == FormattingStatus.WaitToProcess);
    }

    public async Task<ContentFormatTicket> UpdateStatusAsync(
        ContentFormatTicket ticket, FormattingStatus status)
    {
        if (ticket.Status != status)
        {
            ticket.Status = status;
            await _context.SaveChangesAsync();
        }

        return ticket;
    }

    public async Task<ContentFormatTicket> UpdateResultAsync(ContentFormatTicket ticket, FormattedContent result)
    {
        ticket.Result = result;
        await _context.SaveChangesAsync();

        return ticket;
    }
}