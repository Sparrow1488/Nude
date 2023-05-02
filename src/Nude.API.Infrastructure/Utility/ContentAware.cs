using Nude.API.Models.Enums;
using Nude.API.Models.Mangas;
using Nude.Constants;

namespace Nude.API.Infrastructure.Utility;

public static class ContentAware
{
    private static readonly string[] _domains;
    private static readonly IReadOnlyList<SourceAssociationEntry> SourceEntries;
    
    static ContentAware()
    {
        SourceEntries = new List<SourceAssociationEntry>
        {
            new(SourceType.NudeMoon, new List<string> { NudeMoonDefaults.Domain }),
            new(SourceType.HentaiChan, new List<string>(HentaiChanDefaults.Domains))
        };

        _domains = SourceEntries.SelectMany(x => x.AssociationDomains).ToArray();
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
        )?.SourceType;
    }

    private static string? GetDomain(string siteUrl)
    {
        return Uri.TryCreate(siteUrl, UriKind.Absolute, out var uri) ? uri.Host : null;
    }

    private record SourceAssociationEntry(
        SourceType SourceType,
        List<string> AssociationDomains
    );
}