namespace Nude.API.Models.Notifications.Details;

public class FormatTicketProgressDetails : NotificationDetails
{
    public int TicketId { get; set; }
    public int TotalImages { get; set; }
    public int CurrentImage { get; set; }
    public override string Type => nameof(FormatTicketProgressDetails);
}