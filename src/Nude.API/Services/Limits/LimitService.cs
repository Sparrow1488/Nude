using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Models.Users;
using Nude.API.Services.Limits.Handlers;
using Nude.API.Services.Limits.Results;

namespace Nude.API.Services.Limits;

public class LimitService : ILimitService
{
    private readonly IEnumerable<LimitHandler> _limits;

    public LimitService(IEnumerable<LimitHandler> limits)
    {
        _limits = limits;
    }
    
    public IEnumerable<LimitHandler> GetLimits(LimitTarget limitTarget)
    {
        return _limits.Where(x => x.Target == limitTarget);
    }

    public async Task<LimitResult> IsLimitedAsync(LimitTarget limitTarget)
    {
        foreach (var limit in GetLimits(limitTarget))
        {
            var withinLimit = await limit.WithinLimitAsync();
            
            if (withinLimit) continue;
            
            var limitException = new LimitExceededException(
                limit.GetType().Name,
                limit.Target.ToString(),
                "Limit exceeded. Check Data body to get more details"
            );
            return new LimitResult { Exception = limitException };
        }

        return new LimitResult();
    }
}