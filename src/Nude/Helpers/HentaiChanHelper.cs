using AngleSharp.Dom;
using Nude.Exceptions.Parsing;
using Nude.Helpers.Abstractions;

namespace Nude.Helpers;

public class HentaiChanHelper : MangaHelper, IHentaiChanHelper
{
    public string GetIdFromUrl(string mangaUrl)
    {
        var url = Url.Create(mangaUrl);
        var partId = url.Path.Split("/").FirstOrDefault(x => char.IsDigit(x[0]))
                     ?? string.Empty;
        
        var mangaId = string.Join("", partId.TakeWhile(char.IsDigit));
        if (string.IsNullOrWhiteSpace(mangaId))
        {
            throw new NoMangaIdException($"Not found manga id in url {mangaUrl}");
        }

        return mangaId;
    }
}