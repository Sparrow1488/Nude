using System.Security.Claims;
using Nude.API.Models.Claims;

namespace Nude.Data.Infrastructure.Extensions;

public static class ClaimEntryExtensions
{
    public static ICollection<Claim> ToClaims(this IEnumerable<ClaimEntry> entries)
    {
        return entries.Select(
            x => new Claim(x.Type, x.Value, "string", x.Issuer
        )).ToList();
    }
}