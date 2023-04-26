using Nude.API.Infrastructure.Abstractions;

namespace Nude.Bot.Tg.Services.Users.Results;

public class UserSessionResult : ServiceResult<UserSession>
{
    public UserSessionResult(Exception exception) : base(exception)
    {
    }

    public UserSessionResult(UserSession result) : base(result)
    {
    }
}