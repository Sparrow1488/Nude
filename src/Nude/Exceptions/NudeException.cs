namespace Nude.Exceptions;

public class NudeException : Exception
{
    public NudeException()
    {
    }

    public NudeException(string? message) : base(message)
    {
    }
}