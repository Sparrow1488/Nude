namespace Nude.Exceptions.Abstractions;

public abstract class NavigationException : NudeException
{
    public NavigationException()
    {
    }

    public NavigationException(string? message) : base(message)
    {
    }
}