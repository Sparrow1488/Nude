using Nude.API.Models.Abstractions;
using Nude.API.Models.Tickets.Contexts;
using Nude.API.Models.Tickets.Results;
using Nude.API.Models.Tickets.States;
using Nude.API.Models.Tickets.Subscribers;

namespace Nude.API.Models.Tickets;

public class ContentTicket : IEntity
{
    public int Id { get; set; }
    public ReceiveStatus Status { get; set; }
    public ReceiveResult? Result { get; set; }
    public ReceiveContext Context { get; set; } = null!;
    public ICollection<Subscriber> Subscribers { get; set; } = null!;
}