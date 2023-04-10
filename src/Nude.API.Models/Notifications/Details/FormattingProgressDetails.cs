namespace Nude.API.Models.Notifications.Details;

public class FormattingProgressDetails : ContentNotificationDetails
{
    public int TotalImages { get; set; }
    public int CurrentImage { get; set; }
    public override string Type => nameof(FormattingProgressDetails);
}