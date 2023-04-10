using Nude.API.Models.Notifications;
using Nude.API.Services.Notifications.Results;

namespace Nude.API.Services.Notifications;

public interface INotificationService
{
    Task<NotificationResult> NotifyAsync(Notification subject);
}