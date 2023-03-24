using System.Net;

namespace Nude.Navigation.Abstractions;

public class NavigationOptions
{
    public ICollection<Cookie> Cookies { get; } = new List<Cookie>();
}