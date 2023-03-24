using System.Security.Claims;

namespace Nude.Authorization;

public class UserCredentials
{
    public UserCredentials() { }
    
    public UserCredentials(ICollection<Claim> claims)
    {
        Claims = claims;
    }
    
    public Claim? this[string claimType] => Claims.FirstOrDefault(x => x.Type == claimType);

    public ICollection<Claim> Claims { get; } = new List<Claim>();
}