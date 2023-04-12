namespace Nude.API.Infrastructure.Exceptions.Client;

public sealed class SourceNotAvailableException : BadRequestException
{
    public SourceNotAvailableException(string? sourceUrl) : base("Source is not available yet")
    {
        Data.Add("source_url", sourceUrl ?? "unknown_source_url");
    }
}