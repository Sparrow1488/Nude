using System.Security.Claims;
using Nude.Authorization;
using Nude.Authorization.Stores.Models;

namespace Nude.Extensions;

public static class SecureStoreExtensions
{
    public static UserCredentials RevertCredentials(this StoreUserCredentials storeCredentials)
    {
        var claims = storeCredentials.Claims?.RevertClaims() ?? new List<Claim>();
        return new UserCredentials(claims, storeCredentials.Schema);
    }
}