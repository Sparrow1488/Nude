namespace Nude.API.Contracts.Parsing.Responses;

public struct ParsingResultResponse
{
    public string Message { get; set; }
    public string StatusCode { get; set; }
    public string? EntityId { get; set; }
}