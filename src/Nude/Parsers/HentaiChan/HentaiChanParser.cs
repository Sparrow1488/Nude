using AngleSharp.Dom;
using Nude.Models;
using Nude.Navigation.Abstractions;

namespace Nude.Parsers.HentaiChan;

public sealed class HentaiChanParser : IHentaiChanParser
{
    private readonly IWebNavigator _navigator;

    internal HentaiChanParser(IWebNavigator navigator)
    {
        _navigator = navigator;
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