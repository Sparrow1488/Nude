namespace Nude.Bot.Tg.Services.Limits.Results;

public sealed class LimitCheckResult
{
    public LimitStatus Status { get; init; }
    public string? ErrorMessage { get; init; }
}