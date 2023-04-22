using Nude.API.Services.Tickets;
using Nude.API.Services.Users;

namespace Nude.API.Services.Limits.Handlers;

public class ContentTicketCreationLimitHandler : LimitHandler
{
    private readonly IUserSession _session;
    private readonly IContentTicketService _ticketService;

    public ContentTicketCreationLimitHandler(
        IUserSession session,
        IContentTicketService ticketService)
    {
        _session = session;
        _ticketService = ticketService;
    }

    public override LimitTarget Target => LimitTarget.ContentTicketCreation;
    
    public override async Task<bool> WithinLimitAsync()
    {
        var user = await _session.GetUserAsync();
        var userTickets = await _ticketService.GetUserTicketsAsync(user.Id);

        return userTickets.Count <= 2;
    }
}