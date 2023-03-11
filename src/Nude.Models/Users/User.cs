using Nude.Models.Abstractions;

namespace Nude.Models.Users;

public class User : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Credentials Credentials { get; set; }
}