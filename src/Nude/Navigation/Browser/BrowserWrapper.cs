using System.Net;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Nude.Exceptions.Navigation;
using Nude.Extensions;
using PuppeteerSharp;

namespace Nude.Navigation.Browser;

public class BrowserWrapper : IBrowserWrapper
{
    private bool _isDisposed;
    
    private readonly IBrowser _browser;
    private readonly HtmlParser _htmlParser;
    private readonly WaitForSelectorOptions _waitForSelectorOptions;
    private IPage? _page;

    private BrowserWrapper(IBrowser browser)
    {
        _browser = browser;
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
    
    public CookieContainer? Cookies { get; private set; }

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
        return new BrowserWrapper(browser);
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

    public void AddCookies(IEnumerable<Cookie> cookies)
    {
        Cookies ??= new CookieContainer().WithCookies(cookies);
    }

    public void ResetCookies()
    {
        Cookies = null;
    }

    private async Task<IResponse> GoToAsync(string url, string? waitForSelector = null)
    {
        var page = await GetOrCreatePageAsync();
        
        var response = await page.GoToAsync(url, WaitUntilNavigation.DOMContentLoaded);
        if (!string.IsNullOrWhiteSpace(waitForSelector))
        {
            await page.WaitForSelectorAsync(waitForSelector, _waitForSelectorOptions);
        }

        var cookieParams = await page.GetCookiesAsync();
        if (cookieParams != null)
        {
            Cookies = Cookies?.WithCookies(cookieParams.ToCookies());
        }
        
        return response;
    }

    private async Task<IPage> GetOrCreatePageAsync()
    {
        if (_page == null || _page.IsClosed)
        {
            _page = await _browser.NewPageAsync();
            
            if (Cookies != null)
            {
                var cookies = Cookies.GetAllCookies().Select(x => new CookieParam
                {
                    Name = x.Name,
                    Value = x.Value,
                    Path = x.Path,
                    Domain = x.Domain
                });
                await _page.SetCookieAsync(cookies.ToArray());
            }
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