using Nude.API.Infrastructure.Exceptions;
using Nude.API.Infrastructure.Services.Notifications.Results;
using Nude.API.Infrastructure.Services.WebHooks;
using Nude.API.Models.Notifications;
using Nude.API.Models.Tickets.Subscribers;

namespace Nude.API.Infrastructure.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IWebHookService _webHookService;

    public NotificationService(IWebHookService webHookService)
    {
        _webHookService = webHookService;
    }
    
    public async Task<NotificationResult> NotifyAsync(Subscriber subscriber, NotificationSubject subject)
    {
        if (!string.IsNullOrWhiteSpace(subscriber.CallbackUrl))
        {
            var result = await _webHookService.SendAsync(subscriber.CallbackUrl, subject);
            return CreateResult(result.IsSuccess, result.Exception);
        }

        var exception = new UnknownCommunicationTypeException();
        return CreateResult(false, exception);
    }

    private static NotificationResult CreateResult(bool success, Exception? exception)
    {
        return new NotificationResult
        {
            IsSuccess = success,
            Exception = exception
        };
    }
}