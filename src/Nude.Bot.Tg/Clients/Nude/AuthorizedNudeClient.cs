using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
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