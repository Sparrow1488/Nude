using Nude.Models.Abstractions;

namespace Nude.Models.Users;

public class Credentials : IEntity
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string HashedPassword { get; set; }
}