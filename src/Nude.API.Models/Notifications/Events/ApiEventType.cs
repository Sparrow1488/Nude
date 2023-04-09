namespace Nude.API.Models.Notifications.Events;

public enum ApiEventType
{
    Unknown = 0,
    ContentTicketStatusChanged = 0x10,
    FormatTicketStatusChanged = 0x20,
    FormatTicketProgressStageChanged = 0x30,
}