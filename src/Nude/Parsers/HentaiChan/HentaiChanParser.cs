using System.Net;
using AngleSharp.Dom;
using Nude.Models;
using Nude.Navigation.Abstractions;
using Nude.Navigation.Http;

namespace Nude.Parsers.HentaiChan;

public class HentaiChanParser : IHentaiChanParser
{
    private readonly IWebNavigator _navigator;

    public HentaiChanParser(IWebNavigator navigator)
    {
        _navigator = navigator;
    }
    
    public static async Task<IHentaiChanParser> CreateAsync(
        string dleNewPm, 
        string dlePassword, 
        string dleUserId, 
        string phpSessionId)
    {
        var options = new HttpClientOptions();
        options.Cookies.Add(new Cookie("dle_newpm", dleNewPm));
        options.Cookies.Add(new Cookie("dle_password", dlePassword));
        options.Cookies.Add(new Cookie("dle_user_id", dleUserId));
        options.Cookies.Add(new Cookie("PHPSESSID", phpSessionId));

        var navigator = await HttpClientNavigator.CreateAsync(options);
        return new HentaiChanParser(navigator);
    }
    
    public Task<List<Manga>> GetAsync(int offset, int take)
    {
        throw new NotImplementedException();
    }

    public async Task<Manga> GetByUrlAsync(string urlString)
    {
        var document = await _navigator.GetDocumentAsync(urlString);
        return new Manga
        {
            Title = GetTitle(document)
        };
    }

    private static string GetTitle(IDocument previewDocument)
    {
        var titleLink = previewDocument.QuerySelector("a.title_top_a");
        return titleLink?.TextContent ?? "null";
    }
}