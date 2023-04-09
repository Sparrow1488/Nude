using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Services.Background;
using Nude.API.Models.Mangas;
using Nude.API.Models.Notifications;
using Nude.API.Models.Notifications.Details;
using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.States;
using Nude.API.Services.Notifications;
using Nude.API.Services.Stealers;
using Nude.API.Services.Stealers.Results;
using Nude.API.Services.Tickets;

namespace Nude.API.Services.Workers;

public class ContentTicketsWorker : IBackgroundWorker
{
    private readonly IContentTicketService _ticketService;
    private readonly ILogger<ContentTicketsWorker> _logger;
    private readonly INotificationService _notificationService;
    private readonly IContentStealerService _contentStealerService;

    public ContentTicketsWorker(
        IContentTicketService ticketService,
        ILogger<ContentTicketsWorker> logger,
        INotificationService notificationService,
        IContentStealerService contentStealerService)
    {
        _ticketService = ticketService;
        _logger = logger;
        _notificationService = notificationService;
        _contentStealerService = contentStealerService;
    }
    
    public async Task ExecuteAsync(BackgroundServiceContext ctx, CancellationToken ctk)
    {
        var ticket = await _ticketService.GetWaitingAsync();

        if (ticket == null)
        {
            _logger.LogDebug("No waiting content-tickets");
            return;
        }

        var contentUrl = ticket.Context.ContentUrl;
        if (!AvailableSources.IsAvailable(contentUrl))
        {
            await _ticketService.UpdateStatusAsync(ticket, ReceiveStatus.Failed);
            _logger.LogWarning("Ticket source url is not yet available ({url})", contentUrl);
            return;
        }
        
        var stealingResult = await _contentStealerService.StealContentAsync(contentUrl);

        LogResult(stealingResult);

        var newTicketStatus = stealingResult.IsSuccess
            ? ReceiveStatus.Success
            : ReceiveStatus.Failed;
        
        await _ticketService.UpdateStatusAsync(ticket, newTicketStatus);
        await _ticketService.UpdateResultAsync(ticket, stealingResult.EntryId.ToString(), "...");

        await NotifySubscribersAsync(ticket, stealingResult);
    }

    private void LogResult(ContentStealingResult result)
    {
        if (result.IsSuccess)
        {
            // TODO: receive stats (time)
            _logger.LogInformation("Content stolen success");
        }
        else
        {
            _logger.LogError(
                result.Exception,
                "Content stealing failed {reason}",
                result.Exception!.Message);
        }
    }

    private async Task NotifySubscribersAsync(ContentTicket ticket, ContentStealingResult result)
    {
        var details = new ContentTicketStatusChangedDetails
        {
            TicketId = ticket.Id,
            Status = ticket.Status,
        };
        
        if (result.EntryType == nameof(MangaEntry))
            details.MangaId = result.EntryId;
        
        var subject = new NotificationSubject { EventDetails = details };
        await _notificationService.NotifyAsync(subject);
    }
}