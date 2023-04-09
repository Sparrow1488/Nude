using Nude.API.Models.Tickets.States;

namespace Nude.API.Models.Notifications.Details;

public class ContentTicketStatusChangedDetails : NotificationDetails
{
    public int TicketId { get; set; }
    public int? MangaId { get; set; }
    public ReceiveStatus Status { get; set; }
    public override string DetailsType => nameof(ContentTicketStatusChangedDetails);
}