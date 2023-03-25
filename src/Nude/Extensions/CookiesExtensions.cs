using System.Net;
using PuppeteerSharp;

namespace Nude.Extensions;

public static class CookiesExtensions
{
    public static IEnumerable<Cookie> ToCookies(this IEnumerable<CookieParam> cookieParams)
    {
        return cookieParams.Select(x => new Cookie(x.Name, x.Value, x.Path, x.Domain));
    }

    public static CookieContainer WithCookies(this CookieContainer container, IEnumerable<Cookie> cookies)
    {
        foreach (var cookie in cookies)
            container.Add(cookie);
        return container;
    }
}