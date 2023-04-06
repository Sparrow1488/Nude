using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Requests.Contexts;

public abstract class ReceiveContext : IEntity
{
    public int Id { get; set; }
}