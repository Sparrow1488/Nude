using System.Globalization;
using Kvyk.Telegraph;
using Kvyk.Telegraph.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nude.API.Infrastructure.Constants.Defaults;
using Nude.API.Infrastructure.Exceptions.Internal;
using Polly;
using Polly.Retry;

namespace Nude.API.Infrastructure.Clients.Telegraph;

public class DefaultTelegraphClient : ITelegraphClient
{
    private const string BaseUrl = "https://telegra.ph";
    
    private readonly IConfiguration _configuration;
    private readonly ILogger<DefaultTelegraphClient> _logger;
    private TelegraphClient _telegraph = null!;
    private readonly AsyncRetryPolicy _retryPolicy;

    public DefaultTelegraphClient(IConfiguration configuration, ILogger<DefaultTelegraphClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(3));
        
        InitTelegraph();
    }

    private void InitTelegraph()
    {
        var accessToken = _configuration[BotDefaults.TelegraphAccessTokenSection];
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new Exception("Telegraph AccessToken not set or invalid!");
        }
        _telegraph = new TelegraphClient { AccessToken = accessToken };
    }

    public async Task<string> CreatePageAsync(string title, string text, IEnumerable<string> images)
    {
        var imagesList = images.ToList();
        
        _logger.LogInformation("Creating tgh page titled '{title}' with {count} images", title, imagesList.Count);

        var nodes = CreateTelegraphNodes(text, imagesList).ToList();
        var page = await _telegraph.CreatePage(title, nodes);
        
        _logger.LogInformation("Created tgh page successes ({url})", page.Url);
        
        return page.Url;
    }

    private static IEnumerable<Node> CreateTelegraphNodes(string text, IEnumerable<string> images)
    {
        var imagePage = 1;
        var tghImages = images.Select(x => Node.ImageFigure(x, imagePage++.ToString()));
        var nodes = new List<Node>
        {
            Node.P(text)
        };
        nodes.AddRange(tghImages);

        return nodes;
    }

    public async Task<string?> UploadFileAsync(string externalFileUrl)
    {
        _logger.LogDebug("Downloading external file url:{url}", externalFileUrl);
        
        using var httpClient = new HttpClient();
        var bytes = await httpClient.GetByteArrayAsync(externalFileUrl);
        
        _logger.LogDebug("Downloaded success");
        _logger.LogDebug("Uploading external file to tgh...");

        using var content = CreateHttpContent(bytes, "image/png");
        
        using var response = await RetryPostAsync(httpClient, "https://telegra.ph/upload", content);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            if (TryDeserializeResponse(json, out var files))
            {
                return BaseUrl + files!.First().Src;
            }

            return null;
        }
        
        _logger.LogError("Failed to upload files on tgh");
        
        throw new FileUploadException("Telegraph can no longer be used as a file storage");
    }

    private static HttpContent CreateHttpContent(byte[] bytes, string mimeType)
    {
        var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
        content.Add(new StreamContent(
                new MemoryStream(bytes)), 
            mimeType,
            "nude-api-file"
        );
        
        return content;
    }

    private Task<HttpResponseMessage> RetryPostAsync(HttpClient client, string url, HttpContent content)
        => _retryPolicy.ExecuteAsync(async () =>
        {
            var message = await client.PostAsync(url, content);
            message.EnsureSuccessStatusCode();
            return message;
        });

    private bool TryDeserializeResponse(
        string json, 
        out TelegraphFileResponse[]? files)
    {
        files = null;

        try
        {
            files = JsonConvert.DeserializeObject<TelegraphFileResponse[]>(json);
            return true;
        }
        catch (Exception)
        {
            var error = JsonConvert.DeserializeObject<TelegraphErrorResponse>(json);
            
            _logger.LogWarning(
                "Failed to upload image. Tgh response message:{message}",
                error.Error
            );
        }

        return false;
    }

    private readonly record struct TelegraphFileResponse(string Src);
    private readonly record struct TelegraphErrorResponse(string Error);
}