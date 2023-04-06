using Nude.API.Models.Abstractions;
using Nude.API.Models.Requests.Contexts;
using Nude.API.Models.Requests.States;
using Nude.API.Models.Requests.Subscribers;

namespace Nude.API.Models.Requests;

public class ReceiveContentRequest : IEntity
{
    public int Id { get; set; }
    public ReceiveStatus Status { get; set; }
    public ReceiveContext Context { get; set; } = null!;
    public ICollection<Subscriber> Subscribers { get; set; } = null!;
}