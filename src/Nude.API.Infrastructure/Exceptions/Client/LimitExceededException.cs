namespace Nude.API.Infrastructure.Exceptions.Client;

public sealed class LimitExceededException : BadRequestException
{
    public LimitExceededException(
        string limit, 
        string limitTarget, 
        string? description, 
        string? message) 
    : base(description, message)
    {
        Data.Add("limit", limit);
        Data.Add("limit_target", limitTarget);
    }
}