using Nude.API.Models.Formats;

namespace Nude.API.Services.Formatters.Results;

public class ContentFormattingResult
{
    public bool IsSuccess { get; set; }
    public Format? FormattedContent { get; set; }
    public Exception? Exception { get; set; }
}