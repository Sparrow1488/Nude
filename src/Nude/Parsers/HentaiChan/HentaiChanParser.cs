using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Nude.Helpers;
using Nude.Helpers.Abstractions;
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

    public IHentaiChanHelper Helper { get; } = new HentaiChanHelper();
    
    public Task<List<Manga>> GetAsync(int offset, int take)
    {
        throw new NotImplementedException();
    }

    public async Task<Manga> GetByUrlAsync(string urlString)
    {
        var document = await _navigator.GetDocumentAsync(urlString);
        return new Manga
        {
            Title = GetTitle(document),
            Author = GetAuthor(document),
            ExternalId = Helper.GetIdFromUrl(urlString),
            OriginUrl = urlString,
            Tags = GetTags(document).ToList()
        };
    }

    private static string GetTitle(IDocument previewDocument)
    {
        var titleLink = previewDocument.QuerySelector("a.title_top_a");
        return titleLink?.TextContent ?? "null";
    }

    private static string GetAuthor(IDocument previewDocument)
    {
        var authorRow = GetMangaInfoRow(previewDocument, "Автор");
        return authorRow?.QuerySelector("h2 a")?.TextContent ?? "";
    }

    private static IElement? GetMangaInfoRow(IDocument previewDocument, string itemName)
    {
        var infoItems = previewDocument.QuerySelectorAll("#info_wrap .row");
        return infoItems.FirstOrDefault(x =>
            x.QuerySelector(".item")?.TextContent.ToUpper().Contains(itemName.ToUpper()) ?? false);
    }

    private static IEnumerable<string> GetTags(IDocument previewDocument)
    {
        var tagElements = previewDocument.QuerySelectorAll("li.sidetag");
        return tagElements.Select(x => 
            x.QuerySelectorAll("a").LastOrDefault()?.TextContent ?? "")
            .Distinct();
    }

    public void Dispose()
    {
        _navigator.Dispose();
    }
}