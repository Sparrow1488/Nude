using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Nude.Exceptions.Navigation;
using PuppeteerSharp;

namespace Nude.Navigation.Browser;

public class BrowserWrapper : IBrowserWrapper
{
    private bool _isDisposed;
    
    private readonly IBrowser _browser;
    private readonly BrowserOptions _options;
    private readonly HtmlParser _htmlParser;
    private readonly WaitForSelectorOptions _waitForSelectorOptions;
    private IPage? _page;

    // TODO: logger
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

    public Task<IDocument> GetDocumentAsync(string url)
    {
        return GetDocumentAsync(url, string.Empty);
    }

    public async Task<IDocument> GetDocumentAsync(string url, string waitSelector)
    {
        var text = await GetTextAsync(url, waitSelector);
        return await _htmlParser.ParseDocumentAsync(text);
    }

    public Task<string> GetTextAsync(string url)
    {
        return GetTextAsync(url, string.Empty);
    }
    
    public async Task<string> GetTextAsync(string url, string waitSelector)
    {
        var response = await GoToAsync(url, waitSelector);
        return await response.TextAsync()
               ?? throw new EmptyResponseException($"Empty text response on request to {url}");
    }

    public async Task<(string? html, int status)> GetTextWithStatusAsync(string url)
    {
        var response = await GoToAsync(url, string.Empty);
        var text = await response.TextAsync();
        return (text, (int)response.Status);
    }

    private async Task<IResponse> GoToAsync(string url, string? waitForSelector = null)
    {
        // TODO: retry policy
        var page = await GetOrCreatePageAsync();
        var response = await page.GoToAsync(url, WaitUntilNavigation.DOMContentLoaded);
        if (!string.IsNullOrWhiteSpace(waitForSelector))
        {
            await page.WaitForSelectorAsync(waitForSelector, _waitForSelectorOptions);
        }
        return response;
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