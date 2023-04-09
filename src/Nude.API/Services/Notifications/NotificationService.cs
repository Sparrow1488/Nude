using Nude.API.Infrastructure.Exceptions;
using Nude.API.Models.Notifications;
using Nude.API.Models.Tickets.Subscribers;
using Nude.API.Services.Notifications.Results;
using Nude.API.Services.Subscribers;
using Nude.API.Services.WebHooks;

namespace Nude.API.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IWebHookService _webHookService;
    private readonly ISubscribersService _subscribersService;

    public NotificationService(
        IWebHookService webHookService,
        ISubscribersService subscribersService)
    {
        _webHookService = webHookService;
        _subscribersService = subscribersService;
    }
    
    public async Task<NotificationResult> NotifyAsync(Subscriber subscriber, NotificationSubject subject)
    {
        if (!string.IsNullOrWhiteSpace(subscriber.CallbackUrl))
        {
            var result = await _webHookService.SendAsync(subscriber.CallbackUrl, subject);
            if (result.IsSuccess)
            {
                await _subscribersService.DeleteAsync(subscriber);
            }
            return CreateResult(result.IsSuccess, result.Exception);
        }

        var exception = new UnknownCommunicationTypeException("Supported only WebHooks communication type");
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