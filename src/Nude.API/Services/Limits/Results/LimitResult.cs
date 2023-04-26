namespace Nude.API.Services.Limits.Results;

public class LimitResult
{
    public LimitResult()
    {
        
    }
    
    public LimitResult(string description)
    {
        Description = description;
    }

    public bool Ok => string.IsNullOrWhiteSpace(Description);
    public string? Description { get; }
}