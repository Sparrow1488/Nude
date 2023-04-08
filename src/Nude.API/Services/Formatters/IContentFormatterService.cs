using Nude.API.Models.Formats;

namespace Nude.API.Services.Formatters;

public interface IContentFormatterService
{
    Task<FormattedContent> FormatAsync(
        string title, 
        string text, 
        IEnumerable<string> images,
        FormatType type);
}