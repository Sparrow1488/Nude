namespace Nude.API.Infrastructure.Exceptions;

public class NoJsonConverterException : ApiException
{
    public NoJsonConverterException()
    {
    }

    public NoJsonConverterException(string? message) : base(message)
    {
    }
}