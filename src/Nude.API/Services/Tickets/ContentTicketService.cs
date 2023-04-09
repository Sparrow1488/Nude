using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.Contexts;
using Nude.API.Models.Tickets.Results;
using Nude.API.Models.Tickets.States;
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
    
    public async Task<ContentTicket> CreateAsync(string sourceUrl)
    {
        var request = new ContentTicket
        {
            Status = ReceiveStatus.WaitToProcess,
            Context = new ContentTicketContext
            {
                ContentUrl = sourceUrl
            }
        };

        await _context.AddAsync(request);
        await _context.SaveChangesAsync();

        return request;
    }

    public async Task<ContentTicket> UpdateStatusAsync(ContentTicket ticket, ReceiveStatus status)
    {
        if (ticket.Status != status)
        {
            ticket.Status = status;
            await _context.SaveChangesAsync();
        }
        
        return ticket;
    }

    public async Task<ContentTicket> UpdateResultAsync(ContentTicket ticket, string entityId, string code)
    {
        ticket.Result ??= new ContentResult();

        ticket.Result.Code = code;
        ticket.Result.EntityId = entityId;
        await _context.SaveChangesAsync();

        return ticket;
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
            .FirstOrDefaultAsync(x => x.Context.ContentUrl.Contains(sourceUrl));
    }

    public Task<ContentTicket?> GetWaitingAsync()
    {
        return _context.ContentTickets
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.Status == ReceiveStatus.WaitToProcess);
    }

    // public async Task<Subscriber> SubscribeAsync(ContentTicket ticket, string callback)
    // {
    //     // ArgumentNullException.ThrowIfNull(callback);
    //     //
    //     // var alreadySubscribed = ticket.Subscribers.Any(x => x.CallbackUrl == callback);
    //     //
    //     // if (!alreadySubscribed)
    //     // {
    //     //     var subscriber = new Subscriber
    //     //     {
    //     //         CallbackUrl = callback,
    //     //         ContentTicketId = ticket.Id,
    //     //         NotifyStatus = NotifyStatus.All
    //     //     };
    //     //     
    //     //     await _context.AddAsync(subscriber);
    //     //     await _context.SaveChangesAsync();
    //     //     
    //     //     ticket.Subscribers.Add(subscriber);
    //     // }
    //     //
    //     // return ticket.Subscribers.First(x => x.CallbackUrl == callback);
    // }
}