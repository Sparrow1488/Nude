using Nude.Exceptions.Abstractions;

namespace Nude.Exceptions;

public class AuthorizationException : NudeException
{
    public AuthorizationException()
    {
    }

    public AuthorizationException(string? message) : base(message)
    {
    }
}