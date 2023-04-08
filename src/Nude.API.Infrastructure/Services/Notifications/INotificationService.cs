using Nude.API.Infrastructure.Services.Notifications.Results;
using Nude.API.Models.Notifications;
using Nude.API.Models.Tickets.Subscribers;

namespace Nude.API.Infrastructure.Services.Notifications;

public interface INotificationService
{
    Task<NotificationResult> NotifyAsync(Subscriber subscriber, NotificationSubject subject);
}