namespace Nude.API.Models.Notifications.Details;

public abstract class ContentNotificationDetails : NotificationDetails
{
    public string ContentKey { get; set; } = null!;
}