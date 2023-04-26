namespace Nude.API.Models.Messages.Details;

public class MediaGroupDetails : MessageDetails
{
    public string? MediaGroupId { get; set; }
    public int CurrentMedia { get; set; }
    public int TotalMedia { get; set; }
}