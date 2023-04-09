using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Servers;

public class Server : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? NotificationsCallbackUrl { get; set; }
}