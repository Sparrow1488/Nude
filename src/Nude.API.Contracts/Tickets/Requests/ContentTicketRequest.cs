namespace Nude.API.Contracts.Tickets.Requests;

public struct ContentTicketRequest
{
    public string SourceUrl { get; set; }
    public string? CallbackUrl { get; set; }
}