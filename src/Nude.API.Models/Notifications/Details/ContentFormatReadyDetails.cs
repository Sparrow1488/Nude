using Nude.API.Models.Tickets.States;

namespace Nude.API.Models.Notifications.Details;

public class ContentFormatReadyDetails : NotificationDetails
{
    public string ContentKey { get; set; }
    public FormattingStatus Status { get; set; }
    public override string Type => nameof(ContentFormatReadyDetails);
}