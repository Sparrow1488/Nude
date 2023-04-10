using Nude.API.Models.Mangas;

namespace Nude.API.Infrastructure.Utility;

public static class EntryTypeDetector
{
    public static string ByContentUrl(string contentUrl)
    {
        return nameof(MangaEntry);
    }
}