using System.Text.RegularExpressions;
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

    public string GetTextInHtmlTagOrInput(string input)
    {
        const string pattern = "<.*?>(.*?)</.*?>";
        if (!ContainsHtmlTag(input)) return input;
        
        var regex = Regex.Match(input, pattern);
        var result = regex.Groups.Values.LastOrDefault()?.Value;
        return string.IsNullOrWhiteSpace(result) ? input : result;
    }

    private static bool ContainsHtmlTag(string input)
    {
        return input.Contains('>') && input.Contains('<') && input.Contains('/');
    }
}