using System.Net.Http.Json;
using Newtonsoft.Json;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Parsing.Responses;

namespace Nude.Tg.Bot.Clients.Nude;

public class NudeClient : INudeClient
{
    private const string BaseUrl = "http://127.0.0.1:3001";
    
    public async Task<MangaResponse?> GetMangaByUrlAsync(string url)
    {
        using var client = CreateHttpClient();
        var response = await client.GetAsync($"{BaseUrl}/nude-moon/manga?url={url}");
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
        var response = await client.PostAsync($"{BaseUrl}/parsing/requests/new?mangaUrl={mangaUrl}", JsonContent.Create(""));
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