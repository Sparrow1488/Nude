namespace Nude.API.Models.Notifications.Details;

public class ContentFormattedResultDetails : NotificationDetails
{
    public string Id { get; set; }
    public override string DetailsType => nameof(ContentFormattedResultDetails);
}