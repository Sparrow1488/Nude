namespace Nude.API.Models.Notifications.Details;

public class ContentFormatProgressDetails : NotificationDetails
{
    public string ContentKey { get; set; }
    public int TotalImages { get; set; }
    public int CurrentImage { get; set; }
    public override string Type => nameof(ContentFormatProgressDetails);
}