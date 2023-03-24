using System.Security.Claims;

namespace Nude.Authorization;

public class UserCredentials
{
    public UserCredentials(ICollection<Claim> claims, Schema schema)
    {
        Claims = claims;
        Schema = schema;
    }
    
    public Claim? this[string claimType] => Claims.FirstOrDefault(x => x.Type == claimType);

    public ICollection<Claim> Claims { get; }
    public Schema Schema { get; }
}