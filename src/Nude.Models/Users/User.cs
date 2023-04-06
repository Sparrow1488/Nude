using Nude.Models.Abstractions;
using Nude.Models.Users.Accounts;
using Nude.Models.Users.Subscriptions;

namespace Nude.Models.Users;

public class User : IEntity<Guid>
{
    public Guid Id { get; set; }
    public ICollection<Account> Accounts { get; set; }
    public UserSubscription UserSubscription { get; set; }
}