using Nude.Models.Sources;
using Nude.Parsers.Abstractions;

namespace Nude.API.Infrastructure.Services.Resolvers;

public interface IMangaParserResolver
{
    Task<IMangaParser> ResolveByUrlAsync(string mangaUrl);
    Task<IMangaParser> ResolveByTypeAsync(SourceType sourceType);
}