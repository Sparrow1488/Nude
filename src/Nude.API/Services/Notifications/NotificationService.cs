using Nude.API.Models.Notifications;
using Nude.API.Services.Notifications.Results;
using Nude.API.Services.WebHooks;

namespace Nude.API.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly string _notificationsCallbackUrl;
    private readonly IWebHookService _webHookService;

    public NotificationService(
        IConfiguration configuration,
        IWebHookService webHookService)
    {
        _webHookService = webHookService;
        _notificationsCallbackUrl = configuration
            .GetRequiredSection("Notifications:CallbackUrl")
            .Get<string>();
    }
    
    public async Task<NotificationResult> NotifyAsync(Notification subject)
    {
        var result = await _webHookService.SendAsync(_notificationsCallbackUrl, subject);
        return CreateResult(result.IsSuccess, result.Exception);
    }

    private static NotificationResult CreateResult(bool success, Exception? exception)
    {
        return new NotificationResult
        {
            IsSuccess = success,
            Exception = exception
        };
    }

    public void Dispose()
    {
        _webHookService.Dispose();
    }
}