using System.Security.Claims;

namespace Nude.Authorization;

public class UserCredentials
{
    public UserCredentials(ICollection<Claim> claims, string domain, Schema schema)
    {
        Claims = claims;
        Domain = domain;
        Schema = schema;
    }
    
    public Claim? this[string claimType] => Claims.FirstOrDefault(x => x.Type == claimType);

    public ICollection<Claim> Claims { get; }
    public string Domain { get; }
    public Schema Schema { get; }
}