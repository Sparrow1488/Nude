using Nude.API.Models.Abstractions;
using Nude.API.Models.Users;

namespace Nude.API.Models.Claims;

public class ClaimEntry : IEntity
{
    public int Id { get; set; }
    public string Type { get; set; } = null!;
    public string Value { get; set; } = null!;
    public string? Issuer { get; set; }
    public User User { get; set; } = null!;
}