using Nude.API.Models.Users;
using Nude.Bot.Tg.Services.Users.Results;

namespace Nude.Bot.Tg.Services.Users;

public interface IUserManager
{
    Task<UserSessionResult> GetUserSessionAsync(long userId, string username);
    Task<UserCreationResult> CreateAsync(long userId, string username, string accessToken);
    Task<TelegramUser?> FindByUserIdAsync(long userId);
}