using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Services.Background;
using Nude.API.Models.Tickets.States;
using Nude.API.Services.Steal;
using Nude.API.Services.Steal.Results;
using Nude.API.Services.Tickets;

namespace Nude.API.Services.Workers;

public class ContentTicketsWorker : IBackgroundWorker
{
    private readonly IContentTicketService _ticketService;
    private readonly ILogger<ContentTicketsWorker> _logger;
    private readonly IStealContentService _stealContentService;

    public ContentTicketsWorker(
        IContentTicketService ticketService,
        ILogger<ContentTicketsWorker> logger,
        IStealContentService stealContentService)
    {
        _ticketService = ticketService;
        _logger = logger;
        _stealContentService = stealContentService;
    }
    
    public async Task ExecuteAsync(BackgroundServiceContext ctx, CancellationToken ctk)
    {
        var ticket = await _ticketService.GetWaitingAsync();

        if (ticket == null)
        {
            _logger.LogDebug("No waiting tickets");
            return;
        }

        var contentUrl = ticket.Context.ContentUrl;
        if (!AvailableSources.IsAvailable(contentUrl))
        {
            await _ticketService.UpdateStatusAsync(ticket, ReceiveStatus.Failed);
            _logger.LogWarning("Ticket source url is not yet available ({url})", contentUrl);
            return;
        }
        
        var stealingResult = await _stealContentService.StealContentAsync(contentUrl);

        LogResult(stealingResult);

        var newTicketStatus = stealingResult.IsSuccess
            ? ReceiveStatus.Success
            : ReceiveStatus.Failed;
        
        await _ticketService.UpdateStatusAsync(ticket, newTicketStatus);
        await _ticketService.UpdateResultAsync(ticket, stealingResult.EntryId.ToString(), "---");
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
}