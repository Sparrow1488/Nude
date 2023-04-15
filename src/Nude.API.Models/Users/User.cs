using Nude.API.Models.Abstractions;
using Nude.API.Models.Tickets;
using Nude.API.Models.Users.Accounts;

namespace Nude.API.Models.Users;

public class User : IEntity
{
    public int Id { get; set; }
    public ICollection<Account> Accounts { get; set; } = null!;
    public ICollection<ContentTicket> ContentTickets { get; set; } = null!;
}