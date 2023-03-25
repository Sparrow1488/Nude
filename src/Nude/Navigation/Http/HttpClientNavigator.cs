using System.Net;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace Nude.Navigation.Http;

public class HttpClientNavigator : IHttpClientNavigator
{
    public CookieContainer? Cookies { get; private set; }

    public async Task<IDocument> GetDocumentAsync(string url)
    {
        var parser = new HtmlParser();
        var html = await GetTextAsync(url);

        return await parser.ParseDocumentAsync(html);
    }

    public async Task<string> GetTextAsync(string url)
    {
        using var client = CreateHttpClient();
        return await client.GetStringAsync(url);
    }

    public async Task<(string? html, int status)> GetTextWithStatusAsync(string url)
    {
        using var client = CreateHttpClient();
        using var response = await client.GetAsync(url);
        
        var html = await response.Content.ReadAsStringAsync();
        var status = response.StatusCode;
        
        return (html, (int) status);
    }

    public void AddCookies(IEnumerable<Cookie> cookies)
    {
        Cookies ??= new CookieContainer();
        foreach (var cookie in cookies)
            Cookies.Add(cookie);
    }

    public void ResetCookies()
    {
        Cookies = null;
    }

    private HttpClient CreateHttpClient()
    {
        return new HttpClient(new HttpClientHandler
        {
            CookieContainer = Cookies ?? new CookieContainer()
        });
    }

    public void Dispose()
    {
        ResetCookies();
    }
}