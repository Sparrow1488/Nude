using Nude.API.Infrastructure.Abstractions;

namespace Nude.API.Services.Limits.Results;

public class LimitRestrictionsCheckingResult : IServiceResult
{
    public bool IsSuccess => Exception is null;
    public Exception? Exception { get; set; }
}