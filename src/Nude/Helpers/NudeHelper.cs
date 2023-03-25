using AngleSharp.Dom;
using Nude.Exceptions.Parsing;
using Nude.Helpers.Abstractions;

namespace Nude.Helpers;

public class NudeHelper : MangaHelper, INudeHelper
{
    public override string GetIdFromUrl(string mangaUrl)
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