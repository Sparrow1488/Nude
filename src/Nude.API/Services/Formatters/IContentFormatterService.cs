using Nude.API.Models.Formats;

namespace Nude.API.Services.Formatters;

public interface IContentFormatterService
{
    Task<FormattedContent> FormatAsync(string title, IEnumerable<string> сщтеуте);
}