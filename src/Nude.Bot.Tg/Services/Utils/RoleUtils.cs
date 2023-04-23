using Nude.API.Infrastructure.Constants;

namespace Nude.Bot.Tg.Services.Utils;

public static class RoleUtils
{
    public static string GoodPrintRoleName(string claimRole)
    {
        return claimRole switch
        {
            NudeClaims.Roles.Administrator => "Administrator",
            NudeClaims.Roles.User => "User",
            _ => "Unknown"
        };
    }
}