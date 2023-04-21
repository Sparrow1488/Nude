using System.Net;
using Nude.Constants;
using Nude.Exceptions;
using Nude.Extensions;
using Nude.Parsers.HentaiChan;

namespace Nude.Authorization.Handlers;

public class HentaiChanAuthHandler : IAuthHandler<IHentaiChanParser>
{
    public async Task<UserCredentials> AuthorizeAsync(string login, string password)
    {
        var cookies = new CookieContainer();
        using var client = CreateHttpClient(cookies);
        using var request = CreateAuthorizationRequest(login, password);

        using var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var claims = cookies.GetAllCookies().ToClaims();

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