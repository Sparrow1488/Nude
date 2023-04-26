using Nude.API.Infrastructure.Abstractions;
using Nude.API.Models.Users;

namespace Nude.Bot.Tg.Services.Users.Results;

public class UserCreationResult : ServiceResult<TelegramUser>
{
    public UserCreationResult(Exception exception) : base(exception)
    {
    }

    public UserCreationResult(TelegramUser result) : base(result)
    {
    }
}