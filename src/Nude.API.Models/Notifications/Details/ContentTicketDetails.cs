using Nude.API.Models.Tickets.States;

namespace Nude.API.Models.Notifications.Details;

public class ContentTicketDetails : NotificationDetails
{
    public ReceiveStatus ReceiveStatus { get; set; }
    public override string DetailsType => nameof(ContentTicketDetails);
}