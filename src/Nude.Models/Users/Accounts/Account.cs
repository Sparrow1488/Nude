using Nude.Models.Abstractions;

namespace Nude.Models.Users.Accounts;

public abstract class Account : IAuditable<Guid>
{
    public Guid Id { get; set; }
    public User Owner { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}