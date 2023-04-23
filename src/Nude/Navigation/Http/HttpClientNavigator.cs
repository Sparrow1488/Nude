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
        const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36 Edg/111.0.1661.51";
        var client = new HttpClient(new HttpClientHandler
        {
            CookieContainer = Cookies ?? new CookieContainer(),
        });
        
        client.Timeout = TimeSpan.FromSeconds(25);
        client.DefaultRequestHeaders.Add("User-Agent", userAgent);
        return client;
    }

    public void Dispose()
    {
        ResetCookies();
    }
}