namespace Nude.Helpers;

public interface INudeHelper
{
    string GetIdFromUrl(string mangaUrl);
    string GetTextInHtmlTagOrInput(string input);
}