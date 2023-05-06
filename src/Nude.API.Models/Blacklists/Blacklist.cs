using Nude.API.Models.Abstractions;
using Nude.API.Models.Tags;
using Nude.API.Models.Users;

namespace Nude.API.Models.Blacklists;

public class Blacklist : IEntity
{
    public int Id { get; set; }
    public User User { get; set; } = null!;
    public int UserId { get; set; }
    public ICollection<Tag> Tags { get; set; } = null!;
}