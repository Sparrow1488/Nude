using Nude.API.Models.Abstractions;
using Nude.API.Models.Users;

namespace Nude.API.Models.Tickets;

public class ContentTicket : IEntity
{
    public int Id { get; set; }
    public string ContentKey { get; set; } = null!;
    public string ContentUrl { get; set; } = null!;
    public User User { get; set; } = null!;
}