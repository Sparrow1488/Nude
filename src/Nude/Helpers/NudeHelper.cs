using AngleSharp.Dom;
using Nude.Exceptions.Parsing;

namespace Nude.Helpers;

public class NudeHelper : INudeHelper
{
    public string GetIdFromUrl(string mangaUrl)
    {
        var url = Url.Create(mangaUrl);
        var mangaId = string.Join("", url.Path.TakeWhile(char.IsDigit));
        if (string.IsNullOrWhiteSpace(mangaId))
        {
            throw new NoMangaIdException($"Not found manga id in url {mangaUrl}");
        }

        return mangaId;
    }
}