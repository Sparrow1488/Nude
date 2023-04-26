namespace Nude.API.Models.Messages.Details;

public class ContentTicketDetails : MessageDetails
{
    public int TicketId { get; set; }
    public string ContentKey { get; set; } = null!;
}