using System.Net;

namespace Nude.Navigation;

public class BrowserOptions
{
    public ICollection<Cookie> Cookies { get; } = new List<Cookie>();
}