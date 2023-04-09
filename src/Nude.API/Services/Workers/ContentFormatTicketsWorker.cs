using Nude.API.Infrastructure.Services.Background;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Models.Notifications;
using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.States;
using Nude.API.Services.Formatters;
using Nude.API.Services.Mangas;
using Nude.API.Services.Notifications;
using Nude.API.Services.Subscribers;
using Nude.API.Services.Tickets;

#region Rider annotations

// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

#endregion

namespace Nude.API.Services.Workers;

public class ContentFormatTicketsWorker : IBackgroundWorker
{
    private readonly IMangaService _mangaService;
    private readonly ISubscribersService _subscribersService;
    private readonly INotificationService _notificationService;
    private readonly IContentFormatTicketService _ticketService;
    private readonly IContentFormatterService _formatterService;
    private readonly ILogger<ContentFormatTicketsWorker> _logger;

    public ContentFormatTicketsWorker(
        IMangaService mangaService,
        ISubscribersService subscribersService,
        INotificationService notificationService,
        IContentFormatTicketService ticketService,
        IContentFormatterService formatterService,
        ILogger<ContentFormatTicketsWorker> logger)
    {
        _mangaService = mangaService;
        _subscribersService = subscribersService;
        _notificationService = notificationService;
        _ticketService = ticketService;
        _formatterService = formatterService;
        _logger = logger;
    }
    
    public async Task ExecuteAsync(BackgroundServiceContext ctx, CancellationToken ctk)
    {
        var ticket = await _ticketService.GetWaitingAsync();

        if (ticket == null)
        {
            _logger.LogDebug("No waiting format-tickets");
            return;
        }
        
        _logger.LogInformation("Starting process format-ticket {id}", ticket.Id);

        if (ticket.Context.EntityType == nameof(MangaEntry))
        {
            var manga = await _mangaService.GetByIdAsync(int.Parse(ticket.Context.EntityId));
            var images = manga!.Images.Select(x => x.Url.Value);
            
            images = new List<string>() { images.First() }; // TODO: У МЕНЯ ХУЕВЫЙ ИНЕТ!!! УДАЛИТЬ!!!

            var format = await _formatterService.FormatAsync(
                manga.Title,
                "With Love By Nude",
                images,
                FormatType.Telegraph);

            await _mangaService.AddFormatAsync(manga, format);

            await _ticketService.UpdateStatusAsync(ticket, FormattingStatus.Success);
            await _ticketService.UpdateResultAsync(ticket, format);

            await NotifySubscribersAsync(ticket);
            
            return;
        }

        throw new NotImplementedException();
    }

    private async Task NotifySubscribersAsync(ContentFormatTicket ticket)
    {
        var ticketId = ticket.Id.ToString();
        const string ticketType = nameof(ContentFormatTicket);

        var subject = new NotificationSubject
        {
            EntityId = ticketId,
            EntityType = ticketType,
            Status = ticket.Status.ToString(),
            Details = null
        };
        
        var subs = await _subscribersService.FindAsync(ticketId, ticketType);
        foreach (var sub in subs)
        {
            await _notificationService.NotifyAsync(sub, subject);
        }
    }
}