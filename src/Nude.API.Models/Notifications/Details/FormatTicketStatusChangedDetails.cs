using Nude.API.Models.Tickets.States;

namespace Nude.API.Models.Notifications.Details;

public class FormatTicketStatusChangedDetails : NotificationDetails
{
    public int? MangaId { get; set; }
    public FormattingStatus Status { get; set; }
    public override string Type => nameof(FormatTicketStatusChangedDetails);
}