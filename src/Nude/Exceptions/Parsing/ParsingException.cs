using Nude.Exceptions.Abstractions;

namespace Nude.Exceptions.Parsing;

public class ParsingException : NudeException
{
    public ParsingException()
    {
    }

    public ParsingException(string? message) : base(message)
    {
    }
}