using Nude.API.Models.Abstractions;
using Nude.API.Models.Tickets.States;

namespace Nude.API.Models.Tickets.Subscribers;

public class Subscriber : IEntity
{
    public int Id { get; set; }
    public NotifyStatus NotifyStatus { get; set; }
    public string? CallbackUrl { get; set; }

    public int? ContentTicketId { get; set; }
}