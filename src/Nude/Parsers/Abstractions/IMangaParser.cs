using Nude.Models;

namespace Nude.Parsers.Abstractions;

public interface IMangaParser
{
    Task<List<Manga>> GetAsync(int offset, int take);
    Task<Manga> GetByUrlAsync(string urlString);
}