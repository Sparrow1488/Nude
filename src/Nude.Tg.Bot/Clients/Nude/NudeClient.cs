using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Parsing.Responses;

namespace Nude.Tg.Bot.Clients.Nude;

public class NudeClient : INudeClient
{
    private readonly string _baseUrl;
    
    public NudeClient(IConfiguration configuration)
    {
        _baseUrl = configuration["Nude.API:BaseUrl"] ?? throw new Exception("No Nude.API BaseUrl in config");
    }
    
    public async Task<MangaResponse?> GetMangaByUrlAsync(string url)
    {
        using var client = CreateHttpClient();
        var response = await client.GetAsync($"{_baseUrl}/nude-moon/manga?url={url}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MangaResponse>(json);
        }

        return null;
    }

    public async Task<ParsingResponse> CreateParsingRequestAsync(string mangaUrl, string callback)
    {
        using var client = CreateHttpClient();
        var response = await client.PostAsync($"{_baseUrl}/parsing/requests/new?mangaUrl={mangaUrl}", JsonContent.Create(""));
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ParsingResponse>(json);
        }

        throw new Exception("Че-то пошло не так при создании запроса на парсинг");
    }

    private HttpClient CreateHttpClient()
    {
        return new HttpClient();
    }
}