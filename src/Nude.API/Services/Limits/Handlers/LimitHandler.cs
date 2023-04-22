namespace Nude.API.Services.Limits.Handlers;

public abstract class LimitHandler
{
    public abstract LimitTarget Target { get; }
    
    public abstract Task<bool> WithinLimitAsync();
}