using System.Collections;
using System.Diagnostics;
using Nude.API.Infrastructure.Services.Background;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Models.Notifications;
using Nude.API.Models.Notifications.Details;
using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.States;
using Nude.API.Services.Formatters;
using Nude.API.Services.Formatters.Variables;
using Nude.API.Services.Mangas;
using Nude.API.Services.Notifications;
using Nude.API.Services.Tickets;

#region Rider annotations

// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

#endregion

namespace Nude.API.Services.Workers;

public class ContentFormatTicketsWorker : IBackgroundWorker
{
    private readonly Stopwatch _stopwatch;
    private const string CurrentDiagnosticMethodName = nameof(ContentFormatTicketsWorker);
    
    private readonly IMangaService _mangaService;
    private readonly INotificationService _notificationService;
    private readonly IContentFormatTicketService _ticketService;
    private readonly IContentFormatterService _formatterService;
    private readonly ILogger<ContentFormatTicketsWorker> _logger;

    public ContentFormatTicketsWorker(
        IMangaService mangaService,
        INotificationService notificationService,
        IContentFormatTicketService ticketService,
        IContentFormatterService formatterService,
        ILogger<ContentFormatTicketsWorker> logger)
    {
        _mangaService = mangaService;
        _notificationService = notificationService;
        _ticketService = ticketService;
        _formatterService = formatterService;
        _logger = logger;
        _stopwatch = new Stopwatch();
    }
    
    public async Task ExecuteAsync(BackgroundServiceContext ctx, CancellationToken ctk)
    {
        try
        {
            var ticket = await _ticketService.GetWaitingAsync();

            if (ticket == null)
            {
                _logger.LogDebug("No waiting format-tickets");
                return;
            }

            StartStopwatchDiagnostics();

            _logger.LogInformation("Starting process format-ticket {id}", ticket.Id);

            if (ticket.Context.EntityType == nameof(MangaEntry))
            {
                var manga = await _mangaService.GetByIdAsync(int.Parse(ticket.Context.EntityId));
                var images = manga!.Images.Select(x => x.Url.Value);

                images = new List<string>(images.Take(2)); // TODO: У МЕНЯ ХУЕВЫЙ ИНЕТ!!! УДАЛИТЬ!!!

                _formatterService.FormatProgressUpdated +=
                    variables => OnFormatProgressUpdatedAsync(ticket, variables);

                var format = await _formatterService.FormatAsync(
                    manga.Title,
                    "With Love By Nude",
                    images,
                    FormatType.Telegraph);

                await _mangaService.AddFormatAsync(manga, format);

                await _ticketService.UpdateStatusAsync(ticket, FormattingStatus.Success);
                await _ticketService.UpdateResultAsync(ticket, format);

                var resultDetails = new FormatTicketStatusChangedDetails
                {
                    Status = ticket.Status,
                    MangaId = int.Parse(ticket.Context.EntityId)
                };
                await NotifySubscribersAsync(resultDetails);

                EndStopwatchDiagnostics();

                return;
            }

            _logger.LogError("NotImplementedException");
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex);
        }
    }

    public Task HandleExceptionAsync(Exception exception)
    {
        return Task.CompletedTask;
    }

    private async Task OnFormatProgressUpdatedAsync(ContentFormatTicket ticket, IDictionary variables)
    {
        var details = new FormatTicketProgressDetails
        {
            TicketId = ticket.Id
        };
        
        var totalImages = variables[FormatProgressVariables.TotalImages]?.ToString();
        var currentImage = variables[FormatProgressVariables.CurrentImageProcessing]?.ToString();

        if (!string.IsNullOrWhiteSpace(totalImages))
            details.TotalImages = int.Parse(totalImages);
        if (!string.IsNullOrWhiteSpace(currentImage))
            details.CurrentImage = int.Parse(currentImage);

        await NotifySubscribersAsync(details);
    }

    private async Task NotifySubscribersAsync(NotificationDetails? details = null)
    {
        var subject = new Notification { Details = details };
        await _notificationService.NotifyAsync(subject);
    }

    private void StartStopwatchDiagnostics()
    {
        _stopwatch.Start();
        
        _logger.LogInformation("Diagnostics of the {method} is stated", CurrentDiagnosticMethodName);
    }

    private void EndStopwatchDiagnostics()
    {
        _stopwatch.Stop();

        var elapsed = _stopwatch.Elapsed;
        var elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            elapsed.Hours, elapsed.Minutes, elapsed.Seconds,
            elapsed.Milliseconds / 10);
        
        _logger.LogInformation(
            "{mathod} completed in {time}",
            CurrentDiagnosticMethodName,
            elapsedTime
        );
    }
}