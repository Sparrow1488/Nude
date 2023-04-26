using Nude.API.Infrastructure.Constants;

namespace Nude.Bot.Tg.Services.Utils;

public static class RoleUtils
{
    public static string GoodPrintRoleName(string claimRole)
    {
        return claimRole switch
        {
            NudeClaims.Role.Administrator => "Administrator",
            NudeClaims.Role.User => "User",
            _ => "Unknown"
        };
    }
}