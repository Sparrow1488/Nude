namespace Nude.Exceptions;

public class InvalidMangaUrlException : NudeException
{
    public InvalidMangaUrlException()
    {
    }

    public InvalidMangaUrlException(string? message) : base(message)
    {
    }
}