using Nude.Exceptions.Abstractions;

namespace Nude.Exceptions.Parsing;

public class NoMangaIdException : NudeException
{
    public NoMangaIdException()
    {
    }

    public NoMangaIdException(string? message) : base(message)
    {
    }
}