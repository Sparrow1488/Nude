using Nude.API.Models.Formats;

namespace Nude.API.Contracts.Formats.Responses;

public class TelegraphFormatResponse : FormatResponse
{
    public string Url { get; set; }
    public override FormatType Type => FormatType.Telegraph;
}