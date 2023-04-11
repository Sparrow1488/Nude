namespace Nude.API.Infrastructure.Exceptions;

public class ContentSourceNotAvailableException : ApiException
{
    public ContentSourceNotAvailableException()
    {
    }

    public ContentSourceNotAvailableException(string? message) : base(message)
    {
    }
}