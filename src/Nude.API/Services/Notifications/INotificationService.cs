using Nude.API.Models.Notifications;
using Nude.API.Models.Tickets.Subscribers;
using Nude.API.Services.Notifications.Results;

namespace Nude.API.Services.Notifications;

public interface INotificationService
{
    Task<NotificationResult> NotifyAsync(Subscriber subscriber, NotificationSubject subject);
}