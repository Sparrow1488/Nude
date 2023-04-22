using Nude.API.Services.Limits.Handlers;
using Nude.API.Services.Limits.Results;

namespace Nude.API.Services.Limits;

public interface ILimitService
{
    IEnumerable<LimitHandler> GetLimits(LimitTarget limitTarget);
    Task<LimitResult> IsLimitedAsync(LimitTarget limitTarget);
}