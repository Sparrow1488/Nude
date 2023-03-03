using System.Globalization;
using Kvyk.Telegraph;
using Kvyk.Telegraph.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Infrastructure.Constants;
using Polly;

namespace Nude.Tg.Bot.Clients.Telegraph;

public class DefaultTelegraphClient : ITelegraphClient
{
    public const string BaseUrl = "https://telegra.ph";
    
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

    public async Task<string> CreatePageAsync(MangaResponse manga)
    {
        var title = manga.Title.Split("/")[0].Trim();
        _logger.LogInformation($"Creating tgh page titled '{title}' with {manga.Images.Count} images");
        
        var imagePage = 1;
        var images = manga.Images.Select(x => Node.ImageFigure(x, imagePage++.ToString()));
        var nodes = new List<Node>
        {
            Node.P("❤️With love by Nude❤️"),
            Node.P("🤖 Source: nude-moon.org 🤖")
        };
        nodes.AddRange(images);

        var page = await _telegraph.CreatePage(title, nodes);
        _logger.LogInformation("Created tgh page success ({url})", page.Url);
        return page.Url;
    }
    
    public async Task<string> UploadFileAsync(string externalFileUrl)
    {
        _logger.LogInformation("Downloading external file url:{url}", externalFileUrl);
        
        using var httpClient = new HttpClient();
        var bytes = await httpClient.GetByteArrayAsync(externalFileUrl);
        
        _logger.LogInformation("Downloaded success");
        _logger.LogInformation("Uploading external file to tgh...");

        using var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
        content.Add(new StreamContent(
            new MemoryStream(bytes)), 
            GetFileType(externalFileUrl), 
            "nude-api-file");
        
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(3));

        var pollingRetryCount = 0;
        while (pollingRetryCount++ < 5)
        {
            var result = await retryPolicy.ExecuteAsync(
                () => httpClient.PostAsync("https://telegra.ph/upload", content));
            if (result.IsSuccessStatusCode)
            {
                var fileJson = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<TelegraphFileResponse[]>(fileJson);
                return BaseUrl + response!.First().Src;
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
    
    public struct TelegraphFileResponse
    {
        public string Src { get; set; }
    }
}