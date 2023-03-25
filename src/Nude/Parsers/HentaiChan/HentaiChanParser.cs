using AngleSharp.Dom;
using Newtonsoft.Json.Linq;
using Nude.Exceptions.Parsing;
using Nude.Helpers;
using Nude.Helpers.Abstractions;
using Nude.Models;
using Nude.Navigation.Abstractions;
using Nude.Parsers.Abstractions;

namespace Nude.Parsers.HentaiChan;

public sealed class HentaiChanParser : IHentaiChanParser
{
    private readonly IWebNavigator _navigator;

    internal HentaiChanParser(IWebNavigator navigator)
    {
        _navigator = navigator;
    }

    public IHentaiChanHelper Helper { get; } = new HentaiChanHelper();
    MangaHelper IMangaParser.Helper => (MangaHelper) Helper;

    public Task<List<Manga>> GetAsync(int offset, int take)
    {
        throw new NotImplementedException();
    }

    public async Task<Manga> GetByUrlAsync(string urlString)
    {
        if (!ValidateInputUrl(urlString))
        {
            throw new InvalidMangaUrlException();
        }
        
        urlString = GetValidPreviewUrl(urlString);
        
        using var document = await _navigator.GetDocumentAsync(urlString);
        return new Manga
        {
            Title = GetTitle(document),
            Author = GetAuthor(document),
            ExternalId = Helper.GetIdFromUrl(urlString),
            OriginUrl = urlString,
            Tags = GetTags(document).ToList(),
            Images = await GetImagesAsync(GetReadUrlRequired(urlString))
        };
    }

    private static bool ValidateInputUrl(string url)
    {
        return url.Contains(".hentaichan.");
    }

    private static string GetValidPreviewUrl(string inputUrl)
    {
        return inputUrl.Replace("/online/", "/manga/");
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

    private static string GetReadUrlRequired(string mangaUrl)
    {
        return mangaUrl.Replace("/manga/", "/online/");
    }

    private async Task<List<string>> GetImagesAsync(string readDocumentUrl)
    {
        using var readDocument = await _navigator.GetDocumentAsync(readDocumentUrl);
        var dataScript = readDocument.Scripts.ToList().Select(x => x.TextContent)
            .FirstOrDefault(x => x.Contains("createGallery(data)"));

        var images = new List<string>();

        if (!string.IsNullOrWhiteSpace(dataScript))
        {
            var startIndex = dataScript.IndexOf("\"fullimg\":");
            var jsonArray = string.Join("", dataScript.Skip(startIndex).TakeWhile(x => x != '}'))
                .Replace("'", "\"")
                .Replace("\"fullimg\":", "")
                .Replace("}", "");

            var imagesArray = JArray.Parse(jsonArray);
            images = imagesArray.Values<string>().ToList()!;
        }
        
        return images;
    }

    public void Dispose()
    {
        _navigator.Dispose();
    }
}