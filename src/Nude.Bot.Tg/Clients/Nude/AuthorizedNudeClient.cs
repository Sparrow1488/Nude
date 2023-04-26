using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.Extensions.Configuration;
using Nude.API.Contracts.Images.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
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