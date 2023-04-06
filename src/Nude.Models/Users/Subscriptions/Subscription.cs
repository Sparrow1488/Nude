using Nude.Models.Abstractions;

namespace Nude.Models.Users.Subscriptions;

public class Subscription : IEntity<Guid>
{
    public Guid Id { get; set; }
    public SubscriptionType Type { get; set; }
    public ICollection<UserSubscription> UserSubscriptions { get; set; }
}