using System.Net;
using Nude.Authorization;

namespace Nude.Extensions;

public static class UserCredentialsExtensions
{
    public static IEnumerable<Cookie> ToCookies(this UserCredentials credentials)
    {
        return credentials.Claims
            .Select(claim => new Cookie(
                claim.Type, 
                claim.Value, 
                "/", 
                claim.Issuer))
            .ToList();
    }
}