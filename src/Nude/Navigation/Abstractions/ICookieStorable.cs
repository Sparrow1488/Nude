using System.Net;

namespace Nude.Navigation.Abstractions;

public interface ICookieStorable
{
    CookieContainer? Cookies { get; }
    void AddCookies(IEnumerable<Cookie> cookies);
    void ResetCookies();
}