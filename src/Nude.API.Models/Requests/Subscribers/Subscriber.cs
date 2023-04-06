using Nude.API.Models.Abstractions;
using Nude.API.Models.Requests.States;

namespace Nude.API.Models.Requests.Subscribers;

public class Subscriber : IEntity
{
    public int Id { get; set; }
    public NotifyStatus NotifyStatus { get; set; }
    public string? CallbackUrl { get; set; }
}