using Nude.Exceptions.Abstractions;

namespace Nude.Exceptions.Parsing;

public class NudeArgumentException : NudeException
{
    public NudeArgumentException()
    {
    }

    public NudeArgumentException(string? message) : base(message)
    {
    }
}