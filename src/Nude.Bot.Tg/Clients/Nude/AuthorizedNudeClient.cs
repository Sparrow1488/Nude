using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.Extensions.Configuration;
using Nude.API.Contracts.Blacklists.Responses;
using Nude.API.Contracts.Images.Responses;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Tags.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Models.Formats;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Models.Api;
using Nude.Bot.Tg.Services.Users;

namespace Nude.Bot.Tg.Clients.Nude;

public class AuthorizedNudeClient : NudeClient, IAuthorizedNudeClient
{
    private readonly UserSession _session;

    public AuthorizedNudeClient(
        UserSession session,
        IConfiguration configuration
    ) : base(configuration)
    {
        _session = session;
    }
    
    public Task<ApiResult<ContentTicketResponse>> CreateContentTicketAsync(
        ContentTicketRequest request
    ) => PostAsync<ContentTicketRequest, ContentTicketResponse>("/content-tickets", request);

    public Task<ApiResult<ImageResponse>> CreateImageAsync(byte[] data, string filePath)
    {
        var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
        
        var image = new ByteArrayContent(data);
        image.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
        content.Add(image, "file", filePath);

        return PostAsync<ImageResponse>("/images/new", content);
    }

    Task<ApiResult<MangaResponse>> IAuthorizedNudeClient.GetRandomMangaAsync(FormatType? format) =>
        GetRandomMangaAsync(format);

    public Task<ApiResult<BlacklistResponse>> GetBlacklistAsync() =>
        GetAsync<BlacklistResponse>("/users/me/blacklist");

    public Task<ApiResult<BlacklistResponse>> SetDefaultBlacklistAsync() => 
        PostAsync<EmptyRequest, BlacklistResponse>("/users/me/blacklist/reset", new EmptyRequest());

    public Task<ApiResult<TagResponse[]>> SetBlacklistTagsAsync(
        params string[] tags
    ) => PutAsync<TagResponse[]>(
            "/users/me/blacklist/tags?tags=" + string.Join("-", tags)
        );

    public Task<ApiResult<TagResponse[]>> DeleteBlacklistTagsAsync(
        params string[] tags
    ) => DeleteAsync<TagResponse[]>("/users/me/blacklist/tags?tags=" + string.Join("-", tags));

    protected override HttpClient CreateHttpClient()
    {
        var client = base.CreateHttpClient();

        var accessToken = _session.User.AccessToken;
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue(
                "Bearer", 
                accessToken
            );

        return client;
    }

    public override IAuthorizedNudeClient AuthorizeClient(UserSession session) =>
        throw new NotImplementedException();
}