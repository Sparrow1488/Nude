using Nude.Helpers.Abstractions;
using Nude.Parsers.Abstractions;

namespace Nude.Parsers.NudeMoon;

public interface INudeParser : IMangaParser
{
    NudeInfo Info { get; }
    INudeHelper Helper { get; }
    Task<bool> ExistsAsync(string url);
}