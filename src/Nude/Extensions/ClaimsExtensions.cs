using System.Net;
using System.Security.Claims;
using Nude.Authorization.Stores.Models;

namespace Nude.Extensions;

public static class ClaimsExtensions
{
    public static ICollection<StoreClaimEntry> ToStoreEntries(this IEnumerable<Claim> claims)
    {
        return claims.Select(x => new StoreClaimEntry(x.Type, x.Value, x.Issuer)).ToList();
    }

    public static ICollection<Claim> RevertClaims(this IEnumerable<StoreClaimEntry> entries)
    {
        return entries.Select(x => new Claim(x.Type, x.Value, null, x.Issuer)).ToList();
    }
    
    public static ICollection<Claim> ToClaims(this IEnumerable<Cookie> cookies)
    {
        return cookies.Select(x => new Claim(x.Name, x.Value, null, x.Domain)).ToList();
    }
}