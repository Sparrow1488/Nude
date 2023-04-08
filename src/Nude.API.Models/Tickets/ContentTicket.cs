using Nude.API.Models.Abstractions;
using Nude.API.Models.Tickets.Contexts;
using Nude.API.Models.Tickets.Results;
using Nude.API.Models.Tickets.States;

namespace Nude.API.Models.Tickets;

public class ContentTicket : IEntity
{
    public int Id { get; set; }
    public ReceiveStatus Status { get; set; }
    public ContentResult? Result { get; set; }
    public ContentTicketContext Context { get; set; } = null!;
}