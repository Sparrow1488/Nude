using System.Text.RegularExpressions;
using Nude.Helpers.Abstractions;

namespace Nude.Helpers;

public abstract class MangaHelper : IParseTextInHtmlTag, IParseIdFromUrl
{
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

    public abstract string GetIdFromUrl(string url);
}