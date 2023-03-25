using System.Net;
using Nude.Constants;

namespace Nude.Authorization.Cookies;

public class DefaultAuthorizationCookies : AuthorizationCookies
{
    public override ICollection<Cookie> CreateFrom(UserCredentials credentials)
    {
        return credentials.Claims
            .Select(claim => new Cookie(
                claim.Type, 
                claim.Value, 
                "/", 
                credentials.Domain))
            .ToList();
    }
}