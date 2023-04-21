using System.Net;
using System.Text;
using Nude.Constants;
using Nude.Exceptions;
using Nude.Extensions;
using Nude.Parsers.NudeMoon;

namespace Nude.Authorization.Handlers;

public class NudeMoonAuthHandler : IAuthHandler<INudeParser>
{
    public async Task<UserCredentials> AuthorizeAsync(string login, string password)
    {
        var cookies = new CookieContainer();
        using var client = CreateHttpClient(cookies);
        using var request = CreateAuthorizationRequest(login, password);

        using var response = await client.SendAsync(request);
        var bytes = await response.Content.ReadAsByteArrayAsync();
        
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var encoding = Encoding.GetEncoding("windows-1251");
        var text = encoding.GetString(bytes, 0, bytes.Length);
        
        if (response.IsSuccessStatusCode && !text.Contains("Неправильное имя или пароль"))
        {
            var claims = cookies.GetAllCookies().ToClaims();
            return new UserCredentials(claims, Schema.Cookies);
        }

        throw new AuthorizationException("Invalid user credentials");
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
            new("user_name", login),
            new("user_pass", password),
            new("remember_me", "y"),
            new("login", ""),
        };
        var data = new FormUrlEncodedContent(values);

        var authUrl = NudeMoonDefaults.BaseUrl + "/setuser.php";
        var message = new HttpRequestMessage(HttpMethod.Post, authUrl);
        message.Content = data;
        
        return message;
    }
}