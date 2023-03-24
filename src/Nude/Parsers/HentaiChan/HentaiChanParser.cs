using AngleSharp.Dom;
using Nude.Authorization;
using Nude.Authorization.Cookies;
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
    
    public static async Task<IHentaiChanParser> CreateAsync(UserCredentials credentials)
    {
        var options = new HttpClientOptions();
        var cookies = new HentaiChanAuthorizationCookies()
            .CreateFrom(credentials)
            .ToList();
        
        cookies.ForEach(x => options.Cookies.Add(x));

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

    public void Dispose()
    {
        _navigator.Dispose();
    }
}