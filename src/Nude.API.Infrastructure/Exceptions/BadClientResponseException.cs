namespace Nude.API.Infrastructure.Exceptions;

public class BadClientResponseException : ApiException
{
    public BadClientResponseException()
    {
    }

    public BadClientResponseException(string? message) : base(message)
    {
    }
}