using Nude.API.Models.Tickets.States;

namespace Nude.API.Models.Notifications.Details;

public class FormattingStatusDetails : ContentNotificationDetails
{
    public FormattingStatus Status { get; set; }
    public override string Type => nameof(FormattingStatusDetails);
}