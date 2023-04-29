using System.Collections;
using System.Diagnostics;
using Nude.API.Infrastructure.Services.Background;
using Nude.API.Models.Notifications;
using Nude.API.Models.Notifications.Details;
using Nude.API.Models.Tickets.States;
using Nude.API.Services.Formatters;
using Nude.API.Services.Formatters.Variables;
using Nude.API.Services.Notifications;
using Nude.API.Services.Queues;
using Nude.API.Services.Queues.Models;

#region Rider annotations

// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

#endregion

namespace Nude.API.Services.Workers;

public class FormatsWorker : IBackgroundWorker
{
    private readonly Stopwatch _stopwatch;
    private const string CurrentDiagnosticMethodName = nameof(FormatsWorker);

    private readonly IFormatQueue _queue;
    private readonly INotificationService _notificationService;
    private readonly IFormatterService _formatterService;
    private readonly ILogger<FormatsWorker> _logger;

    public FormatsWorker(
        IFormatQueue queue,
        INotificationService notificationService,
        IFormatterService formatterService,
        ILogger<FormatsWorker> logger)
    {
        _queue = queue;
        _notificationService = notificationService;
        _formatterService = formatterService;
        _logger = logger;
        _stopwatch = new Stopwatch();
    }

    private FormatModelAgent? Agent { get; set; }
    
    public async Task ExecuteAsync(BackgroundServiceContext ctx, CancellationToken ctk)
    {
        try
        {
            Agent = await _queue.DequeueAsync();

            if (Agent == null)
            {
                _logger.LogDebug("No waiting format content");
                return;
            }

            StartStopwatchDiagnostics();

            _formatterService.FormatProgressUpdated += FormatProgressUpdated;

            var format = await _formatterService.FormatAsync(
                Agent.Title,
                Agent.Description ?? string.Empty,
                Agent.Images,
                Agent.Type
            );

            await Agent.ReleaseAsync(format);

            var resultDetails = new FormattingStatusDetails
            {
                Status = FormattingStatus.Success,
                ContentKey = Agent.ContentKey
            };
            await NotifySubscribersAsync(resultDetails);

            EndStopwatchDiagnostics();
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex);
        }
        finally
        {
            _notificationService.Dispose();
            _formatterService.FormatProgressUpdated -= FormatProgressUpdated;
        }
    }

    public Task HandleExceptionAsync(Exception exception)
    {
        return Task.CompletedTask;
    }

    private Task FormatProgressUpdated(IDictionary variables)
        => OnFormatProgressUpdatedAsync(Agent!.ContentKey, variables);

    private async Task OnFormatProgressUpdatedAsync(string contentKey, IDictionary variables)
    {
        var details = new FormattingProgressDetails
        {
            ContentKey = contentKey
        };
        
        var totalImages = variables[FormattingVariables.TotalImages]?.ToString();
        var currentImage = variables[FormattingVariables.CurrentImageProcessing]?.ToString();

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