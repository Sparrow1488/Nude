using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Users.Accounts;

public abstract class Account : IEntity
{
    public int Id { get; set; }
}