using Nude.API.Models.Users;

namespace Nude.Bot.Tg.Services.Users;

public class UserSession
{
    public UserSession(TelegramUser user)
    {
        User = user;
    }
    
    public TelegramUser User { get; }
}