using Nude.Exceptions.Abstractions;

namespace Nude.Exceptions.Navigation;

public class EmptyResponseException : NavigationException
{
    public EmptyResponseException()
    {
    }

    public EmptyResponseException(string? message) : base(message)
    {
    }
}