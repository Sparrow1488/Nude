namespace Nude.API.Contracts.Parsing.Requests;

public struct ParsingCreateRequest
{
    public string MangaUrl { get; set; }
    public string? CallbackUrl { get; set; }
}