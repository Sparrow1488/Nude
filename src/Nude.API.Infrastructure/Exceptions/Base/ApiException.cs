namespace Nude.API.Infrastructure.Exceptions.Base;

public class ApiException : Exception
{
    public ApiException()
    {
    }

    public ApiException(string? message) : base(message)
    {
    }
}