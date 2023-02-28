namespace Nude.API.Contracts.Parsing.Requests;

public struct ParsingCreateRequest
{
    public string SourceUrl { get; set; }
    public string? CallbackUrl { get; set; }
}