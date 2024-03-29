using System.Collections;
using Nude.API.Models.Formats;

namespace Nude.API.Services.Formatters;

public interface IFormatterService
{
    event Func<IDictionary, Task> FormatProgressUpdated;
        
    Task<Format> FormatAsync(
        string title, 
        string text, 
        IEnumerable<string> images,
        FormatType type);
}