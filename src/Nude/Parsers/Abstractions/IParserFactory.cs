namespace Nude.Parsers.Abstractions;

public interface IAuthorisedMangaParserFactory<TParser>
where TParser : IMangaParser
{
    Task<TParser> CreateAuthorizedAsync(string login, string password);
}