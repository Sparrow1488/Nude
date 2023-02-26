using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using PuppeteerSharp;

namespace Nude.Navigation;

public class BrowserWrapper : IBrowserWrapper
{
    private bool _isDisposed;
    
    private readonly IBrowser _browser;
    private readonly BrowserOptions _options;
    private readonly HtmlParser _htmlParser;
    private readonly WaitForSelectorOptions _waitForSelectorOptions;
    private IPage? _page;

    private BrowserWrapper(IBrowser browser, BrowserOptions options)
    {
        _browser = browser;
        _options = options;
        _htmlParser = new HtmlParser();
        _waitForSelectorOptions = new WaitForSelectorOptions
        {
            Timeout = 13 * 1000 // 13 seconds
        };
    }

    ~BrowserWrapper()
    {
        if (!_isDisposed)
        {
            Dispose();
        }
    }

    public static async Task<IBrowserWrapper> CreateAsync(BrowserOptions options)
    {
        using var browserFetcher = new BrowserFetcher();
        var revisionInfo = await browserFetcher.GetRevisionInfoAsync();
        if (!revisionInfo.Downloaded)
        {
            await browserFetcher.DownloadAsync();
        }
        
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Timeout = 13 * 1000, // 13 seconds
            Headless = true, 
            Devtools = false
        });
        return new BrowserWrapper(browser, options);
    }

    public async Task<IDocument> GetDocumentAsync(string url)
    {
        var text = await GetTextAsync(url);
        return await _htmlParser.ParseDocumentAsync(text);
    }

    public async Task<IDocument> GetDocumentAsync(string url, string waitSelector)
    {
        var text = await GetTextAsync(url, waitSelector);
        return await _htmlParser.ParseDocumentAsync(text);
    }

    public async Task<string> GetTextAsync(string url)
    {
        var page = await GetOrCreatePageAsync();
        var response = await page.GoToAsync(url);
        return await response.TextAsync()
            ?? throw new Exception("Empty response");
    }

    public async Task<(string? html, int status)> GetTextWithStatusAsync(string url)
    {
        var page = await GetOrCreatePageAsync();
        var response = await page.GoToAsync(url);
        var text = await response.TextAsync();
        return (text, (int)response.Status);
    }

    public async Task<string> GetTextAsync(string url, string waitSelector)
    {
        var page = await GetOrCreatePageAsync();
        var response = await page.GoToAsync(url);
        await page.WaitForSelectorAsync(waitSelector, _waitForSelectorOptions);
        return await response.TextAsync()
               ?? throw new Exception("Empty response");
    }

    private async Task<IPage> GetOrCreatePageAsync()
    {
        if (_page == null || _page.IsClosed)
        {
            _page = await _browser.NewPageAsync();
            var cookieParams = _options.Cookies.Select(x => new CookieParam
            {
                Name = x.Name,
                Value = x.Value,
                Domain = x.Domain
            });
            await _page.SetCookieAsync(cookieParams.ToArray());
        }

        return _page;
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        
        _page?.Dispose();
        _browser.Disconnect();
        _browser.Process.Kill();
        _browser.Dispose();

        _isDisposed = true;
    }
}