using Nude.API.Models.Mangas;
using Nude.Models.Sources;

namespace Nude.API.Infrastructure.Utility;

public static class ContentAware
{
    private static readonly List<string> _domains;
    private static readonly List<SourceAssociationEntry> SourceEntries;
    
    static ContentAware()
    {
        SourceEntries = new List<SourceAssociationEntry>
        {
            new()
            {
                SourceType = SourceType.NudeMoon,
                AssociationDomains = new List<string> { "nude-moon.org" }
            },
            new()
            {
                SourceType = SourceType.HentaiChan,
                AssociationDomains = new List<string> { "y.hentaichan.live", "xxxxx.hentaichan.live" }
            },
        };

        _domains = SourceEntries.SelectMany(x => x.AssociationDomains).ToList();
    }

    public static IEnumerable<string> Domains => _domains;

    public static bool IsSealingAvailable(string resourceUrl)
    {
        var domain = GetDomain(resourceUrl);
        return Domains.Contains(domain ?? resourceUrl);
    }
    
    public static string? DetectEntryTypeByUrl(string contentUrl)
    {
        var sourceType = GetSourceTypeByUrl(contentUrl);
        if (sourceType is SourceType.NudeMoon or SourceType.HentaiChan)
        {
            return nameof(MangaEntry);
        }

        return null;
    }

    private static SourceType? GetSourceTypeByUrl(string url)
    {
        return SourceEntries.FirstOrDefault(
            x => x.AssociationDomains.Contains(GetDomain(url) ?? "")
        ).SourceType;
    }

    private static string? GetDomain(string siteUrl)
    {
        return Uri.TryCreate(siteUrl, UriKind.Absolute, out var uri) ? uri.Host : null;
    }
    
    private struct SourceAssociationEntry
    {
        public SourceType SourceType { get; set; }
        public List<string> AssociationDomains { get; set; }
    }
}