using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Contracts.Tokens.Responses;
using Nude.API.Infrastructure.Configurations.Json;
using Nude.API.Infrastructure.Constants;
using Nude.API.Models.Formats;

namespace Nude.Bot.Tg.Clients.Nude;

public class NudeClient : INudeClient
{
    private readonly string _baseUrl;
    private readonly ILogger<NudeClient> _logger;
    private readonly JsonSerializerSettings _jsonSerializerSettings;
    
    public NudeClient(
        ILogger<NudeClient> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _baseUrl = configuration["Nude.API:BaseUrl"] ?? throw new Exception("No Nude.API BaseUrl in config");

        _jsonSerializerSettings = JsonSettingsProvider.CreateDefault();
    }

    public async Task<MangaResponse?> GetMangaByIdAsync(int id)
    {
        MangaResponse? result = null;
        await GetAsync<MangaResponse>(
            $"/manga/{id}",
            (_, res) => result = res,
            _ => result = null);
        return result;
    }

    public async Task<JwtTokenResponse?> AuthorizeAsync(string username)
    {
        JwtTokenResponse? result = null;
        await PostAsync<EmptyRequest, JwtTokenResponse>(
            $"/auth?username={username}",
            new EmptyRequest(),
            (_, res) => result = res,
            _ => result = null);
        return result;
    }

    public async Task<MangaResponse?> FindMangaByUrlAsync(string sourceUrl, FormatType? format = null)
    {
        MangaResponse? result = null;
        await GetAsync<MangaResponse>(
            $"/manga?sourceUrl={sourceUrl}&format={format}",
            (_, res) => result = res,
            _ => result = null);

        return result;
    }

    public async Task<MangaResponse?> FindMangaByContentKeyAsync(string contentKey, FormatType? format = null)
    {
        MangaResponse? result = null;
        await GetAsync<MangaResponse>(
            $"/manga?contentKey={contentKey}&format={format}",
            (_, res) => result = res,
            _ => result = null);

        return result;
    }

    public async Task<MangaResponse?> GetRandomMangaAsync(FormatType? format = null)
    {
        MangaResponse? response = null;
        await GetAsync<MangaResponse>(
                "/manga/random?format=" + format,
                (_, res) => response = res,
                _ => response = null);
        return response;
    }

    public async Task<ContentTicketResponse?> CreateContentTicket(ContentTicketRequest request)
    {
        ContentTicketResponse? response = null;
        await PostAsync<ContentTicketRequest, ContentTicketResponse>(
            "/content-tickets", 
            request,
            (_,res) => response = res,
            _ => response = null
        );
        return response;
    }

    private async Task GetAsync<TRes>(
        string path,
        Action<HttpResponseMessage, TRes> onSuccess, 
        Action<HttpResponseMessage> onError)
    {
        _logger.LogInformation("");
        
        using var client = CreateHttpClient();
        var uri = _baseUrl + "/" + ApiDefaults.CurrentVersion + path;
        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TRes>(json, _jsonSerializerSettings)
                ?? throw new JsonException("Fail to deserialize response object");
            onSuccess.Invoke(response, result);
        }
        else
        {
            onError.Invoke(response);
        }
    }

    private async Task PostAsync<TReq, TRes>(
        string path,
        TReq request,
        Action<HttpResponseMessage, TRes> onSuccess, 
        Action<HttpResponseMessage> onError)
    {
        using var client = CreateHttpClient();
        HttpContent? content = null;

        if (request is not EmptyRequest)
        {
            var jsonRequest = JsonConvert.SerializeObject(request, _jsonSerializerSettings);
            content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        }
        
        var uri = _baseUrl + "/" + ApiDefaults.CurrentVersion + path;
        var response = await client.PostAsync(uri, content);
        
        LogRequest("POST", path, response.StatusCode.ToString(), null);
        
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TRes>(json, _jsonSerializerSettings)
                         ?? throw new JsonException("Fail to deserialize response object");
            onSuccess.Invoke(response, result);
        }
        else
        {
            onError.Invoke(response);
        }
    }
    
    private static HttpClient CreateHttpClient()
    {
        return new HttpClient();
    }

    private void LogRequest(string method, string path, string status, string? error)
    {
        _logger.LogInformation(
            "Send '{method} {path}' with status result {status} ({error})",
            method,
            path,
            status,
            error ?? "no_error"
        );
    }
    
    private struct EmptyRequest
    {
        
    }
}