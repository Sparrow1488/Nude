using Nude.API.Models.Users;
using Nude.Bot.Tg.Services.Limits.Results;

namespace Nude.Bot.Tg.Services.Limits;

public class RequestsLimitService : IRequestsLimitService
{
    private readonly TimeSpan _secondsDelay = TimeSpan.FromSeconds(0.6);
    private readonly Dictionary<int, List<UserRequest>> _store = new();

    public LimitStatus CheckRequestsLimit(TelegramUser user)
    {
        var requests = PrepareUserStore(user);

        var now = DateTimeOffset.Now;
        var lastRequest = requests.LastOrDefault();
        
        var difference = now - lastRequest?.Time;
        return difference <= _secondsDelay
            ? LimitStatus.LimitIsReached
            : LimitStatus.NoLimited;
    }

    public void AddCompletedRequest(TelegramUser user)
    {
        var requests = PrepareUserStore(user);
        requests.Add(CreateRequest());        
    }

    private List<UserRequest> PrepareUserStore(TelegramUser user)
    {
        if (!_store.TryGetValue(user.Id, out _))
            _store.Add(user.Id, new List<UserRequest>());

        return _store[user.Id];
    }

    private static UserRequest CreateRequest()
    {
        return new UserRequest(DateTimeOffset.Now);
    }

    private record UserRequest(
        DateTimeOffset Time
    );
}