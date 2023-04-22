using Nude.API.Services.Limits.Handlers;
using Nude.API.Services.Limits.Results;

namespace Nude.API.Services.Limits;

public interface ILimitService
{
    IEnumerable<LimitHandler> GetLimits(LimitTarget limitTarget);
    Task<LimitRestrictionsCheckingResult> IsLimitedAsync(LimitTarget limitTarget);
}