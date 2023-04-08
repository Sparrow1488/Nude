namespace Nude.API.Infrastructure.Exceptions;

public class UnknownCommunicationTypeException : ApiException
{
    public UnknownCommunicationTypeException()
    {
    }

    public UnknownCommunicationTypeException(string? message) : base(message)
    {
    }
}