namespace Nude.API.Models.Notifications.Details;

public class FormatProgressDetails : NotificationDetails
{
    public int TotalImages { get; set; }
    public int CurrentImage { get; set; }
    public override string DetailsType => nameof(FormatProgressDetails);
}