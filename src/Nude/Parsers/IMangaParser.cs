using Nude.Models;

namespace Nude.Parsers;

public interface IMangaParser : IDisposable
{
    Task<List<Manga>> GetAsync(int offset, int take);
    Task<Manga> GetByUrlAsync(string urlString);
}