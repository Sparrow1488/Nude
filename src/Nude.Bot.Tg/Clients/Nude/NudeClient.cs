using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Parsing.Requests;
using Nude.API.Contracts.Parsing.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Models.Formats;

namespace Nude.Bot.Tg.Clients.Nude;

public class NudeClient : INudeClient
{
    private readonly string _baseUrl;
    private readonly JsonSerializerSettings _jsonSerializerSettings;
    
    public NudeClient(IConfiguration configuration)
    {
        _baseUrl = configuration["Nude.API:BaseUrl"] ?? throw new Exception("No Nude.API BaseUrl in config");
        _jsonSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };
    }

    public async Task<NewMangaResponse?> GetMangaByIdAsync(int id)
    {
        NewMangaResponse? result = null;
        await GetAsync<NewMangaResponse>(
            $"/manga/{id}",
            (_, res) => result = res,
            _ => result = null);
        return result;
    }

    public async Task<NewMangaResponse?> GetMangaByUrlAsync(string sourceUrl, FormatType format)
    {
        NewMangaResponse? result = null;
        await GetAsync<NewMangaResponse>(
            $"/manga?sourceUrl={sourceUrl}&format={format}",
            (_, res) => result = res,
            _ => result = null);

        return result;
    }

    public Task<NewMangaResponse?> GetRandomMangaAsync()
    {
        throw new NotImplementedException();
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

    public Task<ContentTicketResponse?> GetContentTicketById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<FormatTicketResponse?> CreateFormatTicket(FormatTicketRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<FormatTicketResponse?> GetFormatTicketById(int id)
    {
        throw new NotImplementedException();
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
    private async Task PostAsync<TRec,TRes>(
        string path,
        TRec request,
        Action<HttpResponseMessage, TRes> onSuccess, 
        Action<HttpResponseMessage> onError)
    {
        using var client = CreateHttpClient();
        var response = await client.PostAsJsonAsync(_baseUrl + path, request);
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