using Nude.Models.Abstractions;

namespace Nude.Models.Users.Subscriptions;

public class UserSubscription : IEntity<Guid>
{
    public Guid Id { get; set; }
    public User Owner { get; set; }
    // public Guid OwnerId { get; set; }
    public Subscription Subscription { get; set; }
    public DateTimeOffset IssuedAt { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
}