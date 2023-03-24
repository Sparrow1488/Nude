using Nude.Helpers;
using Nude.Parsers.Abstractions;

namespace Nude.Parsers.NudeMoon;

public interface INudeParser : IMangaParser, IDisposable
{
    NudeInfo Info { get; }
    INudeHelper Helper { get; }
    Task<bool> ExistsAsync(string url);
}