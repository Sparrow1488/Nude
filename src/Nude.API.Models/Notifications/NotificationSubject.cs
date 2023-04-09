using Nude.API.Models.Notifications.Details;

namespace Nude.API.Models.Notifications;

public class NotificationSubject
{
    public string TestProperty { get; set; } = "Test";
    public NotificationDetails? EventDetails { get; set; }
}