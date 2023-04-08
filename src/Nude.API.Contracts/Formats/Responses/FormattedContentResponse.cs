using Nude.API.Models.Formats;

namespace Nude.API.Contracts.Formats.Responses;

public abstract class FormattedContentResponse
{
    public FormatType Type { get; set; }
}