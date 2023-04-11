using Nude.API.Infrastructure.Services.Background;
using Nude.API.Infrastructure.Utility;
using Nude.API.Models.Notifications;
using Nude.API.Models.Notifications.Details;
using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.States;
using Nude.API.Services.Mangas;
using Nude.API.Services.Notifications;
using Nude.API.Services.Stealers;
using Nude.API.Services.Stealers.Results;
using Nude.API.Services.Tickets;

namespace Nude.API.Services.Workers;

public class ContentTicketsWorker : IBackgroundWorker
{
    private readonly IContentTicketService _ticketService;
    private readonly IMangaService _mangaService;
    private readonly ILogger<ContentTicketsWorker> _logger;
    private readonly INotificationService _notificationService;
    private readonly IContentStealerService _contentStealerService;

    public ContentTicketsWorker(
        IContentTicketService ticketService,
        IMangaService mangaService,
        ILogger<ContentTicketsWorker> logger,
        INotificationService notificationService,
        IContentStealerService contentStealerService)
    {
        _ticketService = ticketService;
        _mangaService = mangaService;
        _logger = logger;
        _notificationService = notificationService;
        _contentStealerService = contentStealerService;
    }

    private ContentTicket? Ticket { get; set; }
    private ICollection<ContentTicket> SimilarTickets { get; set; } = null!;
    
    public async Task ExecuteAsync(BackgroundServiceContext ctx, CancellationToken ctk)
    {
        try
        {
            SimilarTickets = await _ticketService.GetSimilarWaitingAsync();
            Ticket = SimilarTickets.FirstOrDefault();

            if (Ticket == null)
            {
                _logger.LogDebug("No waiting content-tickets");
                return;
            }

            var existsContent = await _mangaService.FindByContentKeyAsync(Ticket.ContentKey);
            if (existsContent != null)
            {
                _logger.LogInformation(
                    "Content already exists, key:{url}",
                    existsContent.ContentKey
                );
                
                await NotifySubscribersAsync(Ticket, ReceiveStatus.Success);
                return;
            }

            var contentUrl = Ticket.ContentUrl;
            if (!ContentAware.IsSealingAvailable(contentUrl))
            {
                _logger.LogWarning("Ticket source url is not yet available ({url})", contentUrl);

                await NotifySubscribersAsync(Ticket, ReceiveStatus.Failed);
                return;
            }

            await NotifySubscribersAsync(Ticket, ReceiveStatus.Started);
            
            var stealingResult = await _contentStealerService.StealContentAsync(contentUrl);
            LogResult(stealingResult);

            await NotifySubscribersAsync(Ticket, ReceiveStatus.Success);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex);
        }
        finally
        {
            if (Ticket != null)
            {
                await _ticketService.DeleteRangeAsync(SimilarTickets);
            }
        }
    }

    public async Task HandleExceptionAsync(Exception exception)
    {
        if (Ticket == null) return;

        await NotifySubscribersAsync(Ticket, ReceiveStatus.Failed);
    }

    private void LogResult(ContentStealingResult result)
    {
        if (result.IsSuccess)
        {
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

    private async Task NotifySubscribersAsync(
        ContentTicket ticket,
        ReceiveStatus status)
    {
        var details = new ContentTicketChangedDetails
        {
            TicketId = ticket.Id,
            Status = status,
            ContentKey = ticket.ContentKey
        };
        
        var subject = new Notification { Details = details };
        await _notificationService.NotifyAsync(subject);
    }
}