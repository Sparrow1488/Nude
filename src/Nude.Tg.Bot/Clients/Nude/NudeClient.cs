using System.Net.Http.Json;
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
    
    public async Task<MangaResponse?> GetMangaByUrlAsync(string url)
    {
        using var client = CreateHttpClient();
        var response = await client.GetAsync($"{_baseUrl}/manga?url={url}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MangaResponse>(json, _jsonSerializerSettings);
        }

        return null;
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

    private static HttpClient CreateHttpClient()
    {
        return new HttpClient();
    }
}