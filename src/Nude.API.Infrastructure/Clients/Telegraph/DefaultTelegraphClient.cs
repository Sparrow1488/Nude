using System.Globalization;
using Kvyk.Telegraph;
using Kvyk.Telegraph.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nude.API.Infrastructure.Constants;
using Polly;

namespace Nude.API.Infrastructure.Clients.Telegraph;

public class DefaultTelegraphClient : ITelegraphClient
{
    private const string BaseUrl = "https://telegra.ph";
    
    private readonly IConfiguration _configuration;
    private readonly ILogger<DefaultTelegraphClient> _logger;
    private TelegraphClient _telegraph = null!;

    public DefaultTelegraphClient(IConfiguration configuration, ILogger<DefaultTelegraphClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
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
        
        _logger.LogInformation($"Creating tgh page titled '{title}' with {imagesList.Count} images");

        var nodes = CreateTelegraphNodes(title, imagesList).ToList();
        var page = await _telegraph.CreatePage(title, nodes);
        
        _logger.LogInformation("Created tgh page success ({url})", page.Url);
        
        return page.Url;
    }

    private static IEnumerable<Node> CreateTelegraphNodes(string title, IEnumerable<string> images)
    {
        var imagePage = 1;
        var tghImages = images.Select(x => Node.ImageFigure(x, imagePage++.ToString()));
        var nodes = new List<Node>
        {
            Node.P(title)
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

        using var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
        content.Add(new StreamContent(
            new MemoryStream(bytes)), 
            GetFileType(externalFileUrl), 
            "nude-api-file");
        
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(3));

        var pollingRetryCount = 0;
        while (pollingRetryCount++ < 7)
        {
            var result = await retryPolicy.ExecuteAsync(
                () => httpClient.PostAsync("https://telegra.ph/upload", content));
            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
                if (TryDeserializeResponse(json, out var files, out var error))
                {
                    return BaseUrl + files!.First().Src;
                }

                return null;
            }
            _logger.LogWarning("Failed upload on tgh, status: {status}", result.StatusCode);
        }

        _logger.LogError("Failed to upload files on tgh");
        throw new Exception("Tgh хуета а не файловая БД");
    }

    private static string GetFileType(string fileUrl)
    {
        return "image/png";
    }

    private bool TryDeserializeResponse(
        string json, 
        out TelegraphFileResponse[]? files, 
        out TelegraphErrorResponse? error)
    {
        error = null;
        files = null;

        try
        {
            files = JsonConvert.DeserializeObject<TelegraphFileResponse[]>(json);
            return true;
        }
        catch (Exception)
        {
            error = JsonConvert.DeserializeObject<TelegraphErrorResponse>(json);
            
            _logger.LogWarning(
                "Failed to upload image. Tgh response message:{message}",
                error.Value.Error
            );
        }

        return false;
    }
    
    public struct TelegraphFileResponse
    {
        public string Src { get; set; }
    }
    
    public struct TelegraphErrorResponse
    {
        public string Error { get; set; }
    }
}