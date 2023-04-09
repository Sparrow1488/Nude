using Nude.API.Models.Notifications.Details;

namespace Nude.API.Models.Notifications;

public class NotificationSubject
{
    public string Status { get; set; } = null!;
    public string EntityId { get; set; } = null!;
    public string EntityType { get; set; } = null!;
    public NotificationDetails? Details { get; set; }
}