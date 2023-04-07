namespace Nude.API.Models.Formats;

public abstract class WebContent : FormattedContent
{
    public string Url { get; set; } = null!;
}