using Nude.API.Models.Users;
using Nude.Bot.Tg.Services.Limits.Results;

namespace Nude.Bot.Tg.Services.Limits;

public interface IRequestsLimitService
{
    LimitStatus CheckRequestsLimit(TelegramUser user);
    void AddCompletedRequest(TelegramUser user);
}