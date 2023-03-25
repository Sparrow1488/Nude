namespace Nude.API.Infrastructure.Constants;

public static class AvailableSources
{
    private static readonly List<string> _domains;
    
    static AvailableSources()
    {
        _domains = new List<string>
        {
            "nude-moon.org",
            "y.hentaichan.live",
            "xxxxx.hentaichan.live"
        };
    }

    public static IEnumerable<string> Domains => _domains;

    public static bool IsAvailable(string resourceUrl)
    {
        if (Uri.TryCreate(resourceUrl, UriKind.Absolute, out var uri))
            return Domains.Contains(uri.Host);
        
        return Domains.Contains(resourceUrl);
    }
}