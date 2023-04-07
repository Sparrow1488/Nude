namespace Nude.API.Contracts.Parsing.Requests;

public struct ContentTicketRequest
{
    public string SourceUrl { get; set; }
    public string? CallbackUrl { get; set; }
}