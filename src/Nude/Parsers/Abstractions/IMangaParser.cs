using Nude.Helpers;
using Nude.Models;

namespace Nude.Parsers.Abstractions;

public interface IMangaParser : IDisposable
{
    MangaHelper Helper { get; }
    
    Task<List<Manga>> GetAsync(int offset, int take);
    Task<Manga> GetByUrlAsync(string urlString);
}