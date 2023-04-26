using Nude.API.Infrastructure.Exceptions.Client;
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

    public async Task<LimitRestrictionsCheckingResult> IsLimitedAsync(LimitTarget limitTarget)
    {
        foreach (var limit in GetLimits(limitTarget))
        {
            var withinLimitResult = await limit.WithinLimitAsync();
            
            if (withinLimitResult.Ok) continue;
            
            var limitException = new LimitExceededException(
                limit.GetType().Name,
                limit.Target.ToString(),
                withinLimitResult.Description,
                "Limit exceeded. Check Data body to get more details"
            );
            return new LimitRestrictionsCheckingResult { Exception = limitException };
        }

        return new LimitRestrictionsCheckingResult();
    }
}