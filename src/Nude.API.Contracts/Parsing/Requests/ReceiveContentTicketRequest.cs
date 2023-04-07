namespace Nude.API.Contracts.Parsing.Requests;

public struct ReceiveContentTicketRequest
{
    public string SourceUrl { get; set; }
    public string? CallbackUrl { get; set; }
}