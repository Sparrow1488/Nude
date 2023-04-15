using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Nude.API.Contracts.Errors.Responses;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Contracts.Tokens.Responses;
using Nude.API.Infrastructure.Configurations.Json;
using Nude.API.Infrastructure.Constants;
using Nude.API.Models.Formats;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Models.Api;
using Nude.Bot.Tg.Services.Users;

namespace Nude.Bot.Tg.Clients.Nude;

public class NudeClient : INudeClient
{
    private readonly IConfiguration _configuration;
    private readonly string _baseUrl;
    private readonly JsonSerializerSettings _jsonSerializerSettings;
    
    public NudeClient(IConfiguration configuration)
    {
        _configuration = configuration;
        _baseUrl = configuration["Nude.API:BaseUrl"] 
            ?? throw new Exception("No Nude.API BaseUrl in config");
        
        _baseUrl += "/" + ApiDefaults.CurrentVersion;
        _jsonSerializerSettings = JsonSettingsProvider.CreateDefault();
    }

    public virtual IAuthorizedNudeClient AuthorizeClient(UserSession session)
    {
        return new AuthorizedNudeClient(session, _configuration);
    }

    public Task<ApiResult<JwtTokenResponse>> AuthorizeAsync(
        string username
    ) => PostAsync<EmptyRequest, JwtTokenResponse>($"/auth?username={username}", new EmptyRequest());

    public Task<ApiResult<MangaResponse>> GetMangaByIdAsync(
        int id
    ) => GetAsync<MangaResponse>($"/manga/{id}");

    public Task<ApiResult<MangaResponse>> FindMangaByUrlAsync(
        string sourceUrl,
        FormatType? format = null
    ) => GetAsync<MangaResponse>($"/manga?sourceUrl={sourceUrl}&format={format}");

    public Task<ApiResult<MangaResponse>> FindMangaByContentKeyAsync(
        string contentKey,
        FormatType? format = null
    ) => GetAsync<MangaResponse>($"/manga?contentKey={contentKey}&format={format}");

    public Task<ApiResult<MangaResponse>> GetRandomMangaAsync(
        FormatType? format = null
    ) => GetAsync<MangaResponse>($"/manga/random?format={format}");

    protected async Task<ApiResult<TRes>> GetAsync<TRes>(string path)
        where TRes : struct
    {
        using var client = CreateHttpClient();
        
        var response = await client.GetAsync(_baseUrl + path);
        return await CreateResultByMessageAsync<TRes>(response);
    }

    protected async Task<ApiResult<TRes>> PostAsync<TReq, TRes>(string path, TReq request)
        where TRes : struct
    {
        using var client = CreateHttpClient();
        var content = CreateContent(request);
        
        var response = await client.PostAsync(_baseUrl + path, content);
        return await CreateResultByMessageAsync<TRes>(response);
    }

    private HttpContent CreateContent<TReq>(TReq request)
    {
        var jsonRequest = JsonConvert.SerializeObject(request, _jsonSerializerSettings);
        return new StringContent(jsonRequest, Encoding.UTF8, "application/json");
    }

    private async Task<ApiResult<TRes>> CreateResultByMessageAsync<TRes>(HttpResponseMessage message)
        where TRes : struct
    {
        TRes? result = default;
        ErrorResponse? error = default;
        
        var json = await message.Content.ReadAsStringAsync();

        switch (message.IsSuccessStatusCode)
        {
            case true: result = Deserialize<TRes>(json); break;
            case false: error = Deserialize<ErrorResponse>(json); break;
        }

        return new ApiResult<TRes>(result, error, (int) message.StatusCode);
    }

    private TObject? Deserialize<TObject>(string json)
    {
        return !string.IsNullOrWhiteSpace(json)
            ? JsonConvert.DeserializeObject<TObject>(json, _jsonSerializerSettings)
            : default;
    }
    
    protected virtual HttpClient CreateHttpClient()
    {
        return new HttpClient();
    }
    
    protected struct EmptyRequest
    {
        
    }
}