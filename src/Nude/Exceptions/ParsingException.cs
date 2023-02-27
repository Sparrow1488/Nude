namespace Nude.Exceptions;

public class ParsingException : NudeException
{
    public ParsingException()
    {
    }

    public ParsingException(string? message) : base(message)
    {
    }
}