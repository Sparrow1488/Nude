namespace Nude.API.Infrastructure.Exceptions;

public class MangaCreationException : ApiException
{
    public MangaCreationException() { }

    public MangaCreationException(string? message) : base(message) { }
}