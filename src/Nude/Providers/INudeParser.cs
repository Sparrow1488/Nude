using Nude.Helpers;
using Nude.Models;

namespace Nude.Providers;

public interface INudeParser : IDisposable
{
    NudeInfo Info { get; }
    INudeHelper Helper { get; }
    Task<List<Manga>> GetAsync(int offset, int take);
    Task<Manga> GetByUrlAsync(string urlString);
    Task<bool> ExistsAsync(string url);
}