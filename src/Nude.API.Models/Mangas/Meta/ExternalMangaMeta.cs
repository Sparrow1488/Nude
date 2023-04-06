using Nude.API.Models.Sources;
using Nude.API.Models.Urls;

namespace Nude.API.Models.Mangas.Meta;

public class ExternalMangaMeta : MangaMeta
{
    public override bool IsExternal => true;
    public Source Source { get; set; } = null!;
    public Url SourceUrl { get; set; } = null!;
}