namespace Nude.Exceptions;

public class NudeArgumentException : NudeException
{
    public NudeArgumentException()
    {
    }

    public NudeArgumentException(string? message) : base(message)
    {
    }
}