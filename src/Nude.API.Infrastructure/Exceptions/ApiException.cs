namespace Nude.API.Infrastructure.Exceptions;

public class ApiException : Exception
{
    public ApiException()
    {
    }

    public ApiException(string? message) : base(message)
    {
    }
}