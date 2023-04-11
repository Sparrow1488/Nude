using Nude.API.Models.Tickets.States;

namespace Nude.API.Models.Notifications.Details;

public class ContentTicketChangedDetails : ContentNotificationDetails
{
    public int TicketId { get; set; }
    public ReceiveStatus Status { get; set; }
    public override string Type => nameof(ContentTicketChangedDetails);
}