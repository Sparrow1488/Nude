using System.Net;
using AngleSharp.Dom;

namespace Nude.Navigation.Http;

public class HttpClientNavigator : IHttpClientNavigator
{
    private readonly HttpClientOptions _options;

    private HttpClientNavigator(HttpClientOptions options)
    {
        _options = options;
    }
    
    public static Task<IHttpClientNavigator> CreateAsync(HttpClientOptions options)
    {
        return Task.FromResult((IHttpClientNavigator)new HttpClientNavigator(options));
    }

    public Task<IDocument> GetDocumentAsync(string url)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetTextAsync(string url)
    {
        using var client = CreateHttpClient(_options);
        return await client.GetStringAsync(url);
    }

    public Task<(string? html, int status)> GetTextWithStatusAsync(string url)
    {
        throw new NotImplementedException();
    }

    private static HttpClient CreateHttpClient(HttpClientOptions options)
    {
        var cookies = new CookieContainer();
        foreach (var cookie in options.Cookies)
            cookies.Add(cookie);
        
        return new HttpClient(new HttpClientHandler
        {
            CookieContainer = cookies
        });
    }
    
    public void Dispose()
    {
        
    }
}