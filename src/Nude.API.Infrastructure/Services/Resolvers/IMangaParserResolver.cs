using Nude.Parsers;

namespace Nude.API.Infrastructure.Services.Resolvers;

public interface IMangaParserResolver
{
    bool CanBeResolved(string mangaUrl);
    Task<IMangaParser> ResolveByUrlAsync(string mangaUrl);
}