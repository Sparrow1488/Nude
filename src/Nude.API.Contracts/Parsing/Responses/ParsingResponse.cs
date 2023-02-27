namespace Nude.API.Contracts.Parsing.Responses;

public struct ParsingResponse
{
    public string UniqueId { get; set; }
    public string MangaUrl { get; set; }
    public bool IsAlreadyExists { get; set; }
    public string? Message { get; set; }
    public string Status { get; set; }
}