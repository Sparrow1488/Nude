using Nude.API.Infrastructure.Constants;
using Nude.API.Services.Limits.Results;
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
    
    public override async Task<LimitResult> WithinLimitAsync()
    {
        var user = await _session.GetUserAsync();

        var roleClaim = user.Claims.FirstOrDefault(x => x.Type == NudeClaimTypes.Role);
        if (roleClaim?.Value == NudeClaims.Role.Administrator)
        {
            return new LimitResult();
        }
        
        var userTickets = await _ticketService.GetUserTicketsAsync(user.Id);

        const int maxParallelProcessCount = 1;
        if (userTickets.Count < maxParallelProcessCount)
        {
            return new LimitResult();
        }

        return new LimitResult(
            $"Превышен лимит на обработку содержимого: '{maxParallelProcessCount}'. " +
            "Дождитесь завершения обработки предыдущих запросов"
        );
    }
}