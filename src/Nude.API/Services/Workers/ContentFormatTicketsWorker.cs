using System.Collections;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Nude.API.Infrastructure.Services.Background;
using Nude.API.Models.Formats;
using Nude.API.Models.Notifications;
using Nude.API.Models.Notifications.Details;
using Nude.API.Models.Tickets.States;
using Nude.API.Services.Formatters;
using Nude.API.Services.Formatters.Variables;
using Nude.API.Services.Mangas;
using Nude.API.Services.Notifications;
using Nude.API.Services.Tickets;
using Nude.Data.Infrastructure.Contexts;

#region Rider annotations

// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

#endregion

namespace Nude.API.Services.Workers;

public class ContentFormatTicketsWorker : IBackgroundWorker
{
    private readonly Stopwatch _stopwatch;
    private const string CurrentDiagnosticMethodName = nameof(ContentFormatTicketsWorker);

    private readonly AppDbContext _context;
    private readonly IMangaService _mangaService;
    private readonly INotificationService _notificationService;
    private readonly IContentFormatTicketService _ticketService;
    private readonly IContentFormatterService _formatterService;
    private readonly ILogger<ContentFormatTicketsWorker> _logger;

    public ContentFormatTicketsWorker(
        AppDbContext context,
        IMangaService mangaService,
        INotificationService notificationService,
        IContentFormatTicketService ticketService,
        IContentFormatterService formatterService,
        ILogger<ContentFormatTicketsWorker> logger)
    {
        _context = context;
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
            var mangaId = await _context.Mangas
                .Where(x => !x.Formats.Any(x => x.Type == FormatType.Telegraph))
                .Select(x => x.Id)
                .FirstOrDefaultAsync(ctk);
            
            if (mangaId == 0)
            {
                _logger.LogDebug("No waiting format content");
                return;
            }

            StartStopwatchDiagnostics();

            var manga = await _mangaService.GetByIdAsync(mangaId);
            var images = manga!.Images.Select(x => x.Url.Value);

            images = new List<string>(images.Take(2));

            var contentKey = manga.ContentKey;
            _formatterService.FormatProgressUpdated +=
                variables => OnFormatProgressUpdatedAsync(contentKey, variables);

            var format = await _formatterService.FormatAsync(
                manga.Title,
                "With Love By Nude",
                images,
                FormatType.Telegraph);

            await _mangaService.AddFormatAsync(manga, format);

            var resultDetails = new ContentFormatReadyDetails
            {
                Status = FormattingStatus.Success,
                ContentKey = contentKey
            };
            await NotifySubscribersAsync(resultDetails);

            EndStopwatchDiagnostics();
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

    private async Task OnFormatProgressUpdatedAsync(string contentKey, IDictionary variables)
    {
        var details = new ContentFormatProgressDetails
        {
            ContentKey = contentKey
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