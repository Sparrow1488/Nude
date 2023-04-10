using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Infrastructure.Configurations.Json;
using Nude.API.Models.Formats;

namespace Nude.Bot.Tg.Clients.Nude;

public class NudeClient : INudeClient
{
    private readonly string _baseUrl;
    private readonly JsonSerializerSettings _jsonSerializerSettings;
    
    public NudeClient(IConfiguration configuration)
    {
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

    public async Task<MangaResponse?> GetMangaByUrlAsync(string sourceUrl, FormatType? format = null)
    {
        MangaResponse? result = null;
        await GetAsync<MangaResponse>(
            $"/manga?sourceUrl={sourceUrl}&format={format}",
            (_, res) => result = res,
            _ => result = null);

        return result;
    }

    public async Task<MangaResponse?> GetMangaByContentKeyAsync(string contentKey, FormatType? format = null)
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

    public async Task<ContentTicketResponse?> GetContentTicketById(int id)
    {
        ContentTicketResponse? response = null;
        await GetAsync<ContentTicketResponse>(
            $"/content-tickets/{id}",
            (_, res) => response = res,
            _ => response = null);
        return response;
    }

    public async Task<FormatTicketResponse?> CreateFormatTicket(FormatTicketRequest request)
    {
        FormatTicketResponse? response = null;
        await PostAsync<FormatTicketRequest, FormatTicketResponse>(
            "/format-tickets",
            request,
            (_, res) => response = res,
            _ => response = null);
        return response;
    }

    public async Task<FormatTicketResponse?> GetFormatTicketById(int id)
    {
        FormatTicketResponse? response = null;
        await GetAsync<FormatTicketResponse>(
            $"/format-tickets/{id}",
            (_, res) => response = res,
            _ => response = null);
        return response;
    }


    private async Task GetAsync<TRes>(
        string path,
        Action<HttpResponseMessage, TRes> onSuccess, 
        Action<HttpResponseMessage> onError)
    {
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
            onError.Invoke(response);
        }
    }
    private async Task PostAsync<TRec, TRes>(
        string path,
        TRec request,
        Action<HttpResponseMessage, TRes> onSuccess, 
        Action<HttpResponseMessage> onError)
    {
        using var client = CreateHttpClient();
        var jsonRequest = JsonConvert.SerializeObject(request, _jsonSerializerSettings);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync(_baseUrl + path, content);
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
}