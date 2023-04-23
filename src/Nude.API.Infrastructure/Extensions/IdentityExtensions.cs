using System.Security.Claims;
using Nude.API.Infrastructure.Constants;

namespace Nude.API.Infrastructure.Extensions;

public static class IdentityExtensions
{
    public static string GetRoleRequired(this ClaimsIdentity identity)
    {
        var claim = identity.FindFirst(NudeClaimTypes.Role)
            ?? throw new ArgumentNullException("Role claim not found in identity");

        return claim.Value;
    }
}