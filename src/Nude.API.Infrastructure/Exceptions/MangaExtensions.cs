using Nude.Models;
using Nude.Models.Sources;

namespace Nude.API.Infrastructure.Exceptions;

public static class MangaExtensions
{
    public static SourceType GetSourceType(this Manga manga)
    {
        if (manga.OriginUrl.Contains("nude-moon"))
            return SourceType.NudeMoon;
        if (manga.OriginUrl.Contains("hentaichan"))
            return SourceType.HentaiChan;

        return SourceType.Undefined;
    }
}