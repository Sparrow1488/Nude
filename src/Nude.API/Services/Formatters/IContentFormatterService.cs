using System.Collections;
using Nude.API.Models.Formats;

namespace Nude.API.Services.Formatters;

public interface IContentFormatterService
{
    event Func<IDictionary, Task> FormatProgressUpdated;
        
    Task<FormattedContent> FormatAsync(
        string title, 
        string text, 
        IEnumerable<string> images,
        FormatType type);
}