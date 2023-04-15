using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nude.API.Contracts.Errors.Responses;
using Nude.API.Contracts.Images.Responses;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Infrastructure.Configurations.Json;
using Nude.API.Models.Formats;
using Nude.Bot.Tg.models.Api;

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

    public async Task<ApiResult<List<ImageResponse>>?> GetRandomPicturesByTagsAsync(List<string> tags)
    {
        var result = new ApiResult<List<ImageResponse>>();
        string requestTags = string.Empty;

        foreach(string tag in tags)
        {
            requestTags = $"{requestTags}{tag} ";
        }

        List<ImageResponse>? response = null;
        await GetAsync<List<ImageResponse>>(
            $"/v2/images/booru?tags={requestTags}",
            (_, res) => response = res,
            _ => response = null);
        result.Result = response;
        return result;
    }

    public async Task<ApiResult<MangaResponse>?> GetMangaByIdAsync(int id)
    {
        ApiResult<MangaResponse> result = new ApiResult<MangaResponse>();
        MangaResponse? response = null;
        await GetAsync<MangaResponse>(
            $"/manga/{id}",
            (_, res) => response = res,
            _ => response = null);
        
        result.Result = (MangaResponse)response!;
        return result;
    }

    public async Task<ApiResult<MangaResponse>?> FindMangaByUrlAsync(string sourceUrl, FormatType? format = null)
    {
        ApiResult<MangaResponse> result = new ApiResult<MangaResponse>();
        MangaResponse? response = null;
        await GetAsync<MangaResponse>(
            $"/manga?sourceUrl={sourceUrl}&format={format}",
            (_, res) => response = res,
            _ => response = null);
        result.Result = (MangaResponse)response!;
        return result;
    }

    public async Task<ApiResult<MangaResponse>?> FindMangaByContentKeyAsync(string contentKey, FormatType? format = null)
    {
        ApiResult<MangaResponse> result = new ApiResult<MangaResponse>();
        MangaResponse? response = null;
        await GetAsync<MangaResponse>(
            $"/manga?contentKey={contentKey}&format={format}",
            (_, res) => response = res,
            _ => response = null);

        result.Result = (MangaResponse)response!;
        return result;
    }

    public async Task<ApiResult<MangaResponse>?> GetRandomMangaAsync(FormatType? format = null)
    {
        ApiResult<MangaResponse> result = new ApiResult<MangaResponse>();
        MangaResponse? response = null;
        await GetAsync<MangaResponse>(
                "/manga/random?format=" + format,
                (_, res) => response = res,
                _ => response = null);
        result.Result = (MangaResponse)response!;
        return result;
    }

    public async Task<ApiResult<ContentTicketResponse>?> CreateContentTicket(ContentTicketRequest request)
    {
        ApiResult<ContentTicketResponse> result = new ApiResult<ContentTicketResponse>();
        ContentTicketResponse? response = null;
        await PostAsync<ContentTicketRequest, ContentTicketResponse>(
            "/content-tickets", 
            request,
            (_,res) => response = res,
            _ => response = null
        );
        result.Result = (ContentTicketResponse)response!;
        return result;
    }

    private async Task GetAsync<TRes>(
        string path,
        Action<HttpResponseMessage, TRes> onSuccess, 
        Action<ErrorResponse> onError)
    {
        _logger.LogInformation("");
        
        using var client = CreateHttpClient();
        var response = await client.GetAsync(_baseUrl + path);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TRes>(json, _jsonSerializerSettings)
                ?? throw new JsonException("Fail to deserialize response object");
            onSuccess.Invoke(response, result);
        }
        else
        {
            var json = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ErrorResponse>(json,_jsonSerializerSettings);
            onError.Invoke(res);
        }
    }
    
    private async Task PostAsync<TRec, TRes>(
        string path,
        TRec request,
        Action<HttpResponseMessage, TRes> onSuccess, 
        Action<ErrorResponse> onError)
    {
        using var client = CreateHttpClient();
        var jsonRequest = JsonConvert.SerializeObject(request, _jsonSerializerSettings);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync(_baseUrl + path, content);
        
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
            var json = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ErrorResponse>(json, _jsonSerializerSettings);
            onError.Invoke(res);
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
}