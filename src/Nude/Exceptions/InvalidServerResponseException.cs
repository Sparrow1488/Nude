using Nude.Exceptions.Abstractions;

namespace Nude.Exceptions;

public class InvalidServerResponseException : NudeException
{
    public InvalidServerResponseException()
    {
    }

    public InvalidServerResponseException(string? message) : base(message)
    {
    }
}