namespace Nude.API.Services.Stealers.Results;

public class ContentStealingResult
{
    public bool IsSuccess { get; set; }
    public string? ContentKey { get; set; }
    // public int EntryId { get; set; }
    // public string? EntryType { get; set; }
    public Exception? Exception { get; set; }
}