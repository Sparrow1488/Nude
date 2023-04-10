namespace Nude.API.Services.Stealers.Results;

public class ContentStealingResult
{
    public bool IsSuccess { get; set; }
    public string ContentKey { get; set; } = null!;
    public Exception? Exception { get; set; }
}