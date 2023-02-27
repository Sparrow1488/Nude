using System.Globalization;
using Kvyk.Telegraph;
using Kvyk.Telegraph.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nude.API.Contracts.Manga.Responses;
using Nude.Tg.Bot.Constants;

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
        _logger.LogInformation($"Creating tgh page titled '{manga.Title}' with {manga.Images.Count} images");
        
        var imagePage = 1;
        var images = manga.Images.Select(x => Node.ImageFigure(x, imagePage++.ToString()));
        var nodes = new List<Node>
        {
            Node.P("❤️With love by Nude❤️"),
            Node.P("🤖 Source: nude-moon.org 🤖")
        };
        nodes.AddRange(images);
        
        var page = await _telegraph.CreatePage(manga.Title, nodes);
        _logger.LogInformation("Created tgh page success ({url})", page.Url);
        return page.Url;
    }
    
    public async Task<string> UploadFileAsync(string externalFileUrl)
    {
        using var httpClient = new HttpClient();
        var bytes = await httpClient.GetByteArrayAsync(externalFileUrl);

        using var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
        content.Add(new StreamContent(
            new MemoryStream(bytes)), 
            GetFileType(externalFileUrl), 
            "nude-api-file");
        // using var multipartContent = new MultipartContent();
        // multipartContent.Add(new ByteArrayContent(bytes));
        
        // TODO: polling
        var result = await httpClient.PostAsync("https://telegra.ph/upload", content);
        var fileJson = await result.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<TelegraphFileResponse[]>(fileJson);
        return BaseUrl + response!.First().Src;
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