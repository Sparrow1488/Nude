using System.Net;
using AngleSharp.Dom;
using Nude.Exceptions.Parsing;
using Nude.Helpers;
using Nude.Models;
using Nude.Navigation;

namespace Nude.Parsers;

public class NudeParser : INudeParser
{
    private readonly IBrowserWrapper _browser;

    private const string Domain = "nude-moon.org";
    private const string UrlBase = "https://nude-moon.org";
    
    private const string MangaCardsSelector = "table.news_pic2 tbody td.bg_style1 a";
    private const string ReadButtonSelector = "td span.button a";

    // TODO: logger
    private NudeParser(IBrowserWrapper browser)
    {
        _browser = browser;
        Info = new NudeInfo();
        Helper = new NudeHelper();
    }
    
    public NudeInfo Info { get; }
    public INudeHelper Helper { get; }

    public static Task<NudeParser> CreateAsync(string fusionUser, string phpSessionId)
    {
        return CreateAsync(new BrowserOptions
        {
            Cookies =
            {
                new Cookie("fusion_user", fusionUser, "/", Domain),
                new Cookie("PHPSESSID", phpSessionId, "/", Domain)
            }
        });
    }

    private static async Task<NudeParser> CreateAsync(BrowserOptions options)
    {
        var wrapper = await BrowserWrapper.CreateAsync(options);
        return new NudeParser(wrapper);
    }
    
    public async Task<List<Manga>> GetAsync(int offset, int take)
    {
        if (offset < 0 || take < 0)
            throw new NudeArgumentException("Offset or Take should be greater than zero!");
        
        if(take > Info.ItemsOnPage)
            throw new NudeArgumentException("Take should be lower than max items on page (see Info)!");
        
        var mangaUrl = $"{UrlBase}/all_manga?rowstart={offset}";
        using var allMangaDocument = await _browser.GetDocumentAsync(mangaUrl);
        var mangasCards = allMangaDocument.QuerySelectorAll(MangaCardsSelector);
        
        return await ParseMangasCardsAsync(mangasCards, take);
    }

    private async Task<List<Manga>> ParseMangasCardsAsync(IEnumerable<IElement> mangasCards, int take)
    {
        var list = new List<Manga>();
        
        foreach (var card in mangasCards.Take(take))
        {
            var link = UrlBase + card.GetAttribute("href");
            list.Add(await GetByUrlAsync(link));
        }

        return list;
    }

    public async Task<Manga> GetByUrlAsync(string urlString)
    {
        RequireDomain(urlString);
        urlString = MapUrl(urlString);
        
        var mangaId = Helper.GetIdFromUrl(urlString);
        using var document = await _browser.GetDocumentAsync(urlString);

        if (!CheckMangaDocumentValidation(document))
        {
            throw new InvalidMangaUrlException($"Found invalid manga page ({urlString}). Check input url correct");
        }
        
        return new Manga
        {
            ExternalId = mangaId,
            Title = GetTitleRequired(document),
            Description = GetDescription(document),
            Images = await ParseMangaImagesAsync(GetReadUrlRequired(document)),
            Tags = GetTagsRequired(document),
            Likes = GetLikes(document),
            Author = GetAuthor(document),
            OriginUrl = urlString
        };
    }

    private static void RequireDomain(string input)
    {
        if (!Uri.TryCreate(input, UriKind.Absolute, out var uri))
            throw new InvalidMangaUrlException($"Input cannot convert to Uri");
        if (uri.Host != Domain)
            throw new InvalidMangaUrlException($"Not required domain {Domain}");
    }

    private static string MapUrl(string url)
    {
        var onlineStartIndex = url.IndexOf("-online--", StringComparison.Ordinal);
        if (onlineStartIndex != -1)
        {
            url = url.Remove(onlineStartIndex, "-online".Length).Replace("?row", "");
        }

        return url;
    }

    private static bool CheckMangaDocumentValidation(IDocument mangaDocument)
    {
        const string mangaTitleSelector = "tr td .bg_style1 center h1";
        var titleElement = mangaDocument.QuerySelector(mangaTitleSelector);
        return titleElement is not null;
    }

    public async Task<bool> ExistsAsync(string url)
    {
        var (_, status) =  await _browser.GetTextWithStatusAsync(url);
        return status != 404;
    }

    private static string GetTitleRequired(IDocument mangaPageDocument)
    {
        var titleElement = mangaPageDocument.QuerySelector("td h1");
        if (titleElement is null)
        {
            throw new ParsingException("Failed to parse title");
        }
        return titleElement.InnerHtml;
    }

    private static string GetReadUrlRequired(IDocument mangaPageDocument)
    {
        var readButton = mangaPageDocument.QuerySelector(ReadButtonSelector);
        if (!readButton?.InnerHtml.ToUpper().Contains("ЧИТАТЬ") ?? true)
        {
            throw new ParsingException("Failed to parse read button");
        }
        return UrlBase + readButton.GetAttribute("href") + "?row";
    }

    private List<string> GetTagsRequired(IDocument mangaPageDocument)
    {
        const string tagsSelector = "span.tag-links a";
        return mangaPageDocument.QuerySelectorAll(tagsSelector)
            .Select(x => Helper.GetTextInHtmlTagOrInput(x.InnerHtml))
            .ToList();
    }

    private static string GetDescription(IDocument mangaPageDocument)
    {
        const string descSelector = "div.description";
        return mangaPageDocument.QuerySelector(descSelector)?.InnerHtml ?? "";
    }
    
    private static int GetLikes(IDocument mangaPageDocument)
    {
        const string likesSelector = "#ilike";
        var likesString = mangaPageDocument.QuerySelector(likesSelector)?.InnerHtml ?? "";
        likesString = likesString.Replace("(", "").Replace(")", "");
        return int.TryParse(likesString, out var likes) ? likes : -1;
    }
    
    private static string GetAuthor(IDocument mangaPageDocument)
    {
        const string authorBlockSelector = "div.tbl2";
        var authorElement = mangaPageDocument
            .QuerySelectorAll(authorBlockSelector)
            .FirstOrDefault(x => x.InnerHtml.Contains("Автор"))
            ?.QuerySelector("a");
        return authorElement?.InnerHtml ?? "";
    }

    private async Task<List<string>> ParseMangaImagesAsync(string readMangaUrl)
    {
        const string imagesBoxSelect = "img.textbox";
        using var imagesDocument = await _browser.GetDocumentAsync(readMangaUrl, imagesBoxSelect);
        var imagesElements = imagesDocument.QuerySelectorAll(imagesBoxSelect);
        return imagesElements.Select(x => x.GetAttribute("data-src") ?? "").ToList();
    }

    public void Dispose()
    {
        _browser.Dispose();
    }
}