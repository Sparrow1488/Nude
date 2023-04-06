using Nude.API.Models.Urls;

namespace Nude.API.Models.Formats;

public abstract class WebContentFormat : ContentFormat
{
    public Url Url { get; set; }
}