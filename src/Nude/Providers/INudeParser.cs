using Nude.Models;

namespace Nude.Providers;

public interface INudeParser : IDisposable
{
    NudeInfo Info { get; }
    Task<List<Manga>> GetAsync(int offset, int take);
    Task<Manga> GetByUrlAsync(string url);
}