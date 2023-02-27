namespace Nude.Exceptions.Abstractions;

public abstract class NudeException : Exception
{
    public NudeException()
    {
    }

    public NudeException(string? message) : base(message)
    {
    }
}