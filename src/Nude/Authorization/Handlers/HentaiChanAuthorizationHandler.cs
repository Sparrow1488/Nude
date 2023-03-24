using System.Net;
using System.Security.Claims;
using Nude.Authorization.Stores;
using Nude.Constants;
using Nude.Exceptions;
using Nude.Parsers.HentaiChan;

namespace Nude.Authorization.Handlers;

public class HentaiChanAuthorizationHandler : IAuthorizationHandler<IHentaiChanParser>
{
    private readonly ICredentialsSecureStore _credentialsStore;

    public HentaiChanAuthorizationHandler(ICredentialsSecureStore credentialsStore)
    {
        _credentialsStore = credentialsStore;
    }
    
    public async Task<UserCredentials> AuthorizeAsync(string login, string password)
    {
        var cookies = new CookieContainer();
        using var client = CreateHttpClient(cookies);
        using var request = CreateAuthorizationRequest(login, password);

        using var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            // TODO: create UserCredentialsToken and cache in storage
            
            var claims = cookies
                .GetAllCookies()
                .Select(x => new Claim(x.Name, x.Value)).ToList();

            if (claims.Count < 3)
            {
                throw new AuthorizationException("Invalid user credentials");
            }
        
            return new UserCredentials(claims, Schema.Cookies);
        }

        throw new InvalidServerResponseException("Invalid user credentials");
    }

    private static HttpClient CreateHttpClient(CookieContainer cookies)
    {
        return new HttpClient(new HttpClientHandler
        {
            CookieContainer = cookies
        });
    }

    private static HttpRequestMessage CreateAuthorizationRequest(string login, string password)
    {
        var values = new List<KeyValuePair<string, string>>
        {
            new("login", "submit"),
            new("login_name", login),
            new("login_password", password),
            new("image", "Вход"),
        };
        var data = new FormUrlEncodedContent(values);

        var authUrl = HentaiChanDefaults.BaseUrl;
        var message = new HttpRequestMessage(HttpMethod.Post, authUrl);
        message.Content = data;
        
        return message;
    }
}