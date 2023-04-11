namespace Nude.API.Models.Formats;

public abstract class WebFormat : Format
{
    public string Url { get; set; } = null!;
}