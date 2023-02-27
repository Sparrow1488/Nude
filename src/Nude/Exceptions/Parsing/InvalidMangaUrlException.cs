using Nude.Exceptions.Abstractions;

namespace Nude.Exceptions.Parsing;

public class InvalidMangaUrlException : NudeException
{
    public InvalidMangaUrlException()
    {
    }

    public InvalidMangaUrlException(string? message) : base(message)
    {
    }
}