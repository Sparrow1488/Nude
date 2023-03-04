using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Parsing.Requests;
using Nude.API.Contracts.Parsing.Responses;

namespace Nude.Tg.Bot.Clients.Nude;

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

    public async Task<MangaResponse?> GetMangaByIdAsync(int id)
    {
        MangaResponse? result = null;
        await GetAsync<MangaResponse>(
            $"/manga/{id}",
            (_, res) => result = res,
            _ => result = null);

        return result;
    }

    public async Task<MangaResponse?> GetMangaByUrlAsync(string url)
    {
        MangaResponse? result = null;
        await GetAsync<MangaResponse>(
            $"/manga?url={url}",
            (_, res) => result = res,
            _ => result = null);

        return result;
    }

    public async Task<ParsingResponse?> GetParsingTicketAsync(int id)
    {
        using var client = CreateHttpClient();
        ParsingResponse? result = null;
        await GetAsync<ParsingResponse>(
            $"/parsing/tickets/{id}",
            (_, res) => result = res,
            _ => result = null);

        return result;
    }

    public async Task<ParsingResponse> CreateParsingTicketAsync(string mangaUrl, string callback)
    {
        using var client = CreateHttpClient();
        var requestJson = JsonConvert.SerializeObject(
            new ParsingCreateRequest
            {
                SourceUrl = mangaUrl,
                CallbackUrl = callback
            }, _jsonSerializerSettings);
        var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"{_baseUrl}/parsing/tickets", content);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ParsingResponse>(json, _jsonSerializerSettings);
        }

        throw new Exception($"HttpResponse Status:{response.StatusCode}");
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

    private static HttpClient CreateHttpClient()
    {
        return new HttpClient();
    }
}