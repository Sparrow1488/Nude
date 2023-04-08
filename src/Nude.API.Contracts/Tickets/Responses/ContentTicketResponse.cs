using Nude.API.Models.Tickets.States;

namespace Nude.API.Contracts.Tickets.Responses;

public struct ContentTicketResponse
{
    public int Id { get; set; }
    public ReceiveStatus Status { get; set; }
    public ContentResponse? Result { get; set; }
    public TicketContextResponse Context { get; set; }
}