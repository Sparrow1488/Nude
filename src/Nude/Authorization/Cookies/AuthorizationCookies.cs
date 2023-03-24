using System.Net;

namespace Nude.Authorization.Cookies;

public abstract class AuthorizationCookies
{
    public abstract ICollection<Cookie> CreateFrom(UserCredentials credentials);
}