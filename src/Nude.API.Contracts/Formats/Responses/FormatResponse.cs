using Nude.API.Models.Formats;

namespace Nude.API.Contracts.Formats.Responses;

public abstract class FormatResponse
{
    public abstract FormatType Type { get; }
}