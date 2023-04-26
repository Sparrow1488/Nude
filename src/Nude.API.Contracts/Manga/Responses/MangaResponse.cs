using Nude.API.Contracts.Formats.Responses;
using Nude.API.Contracts.Metas.Responses;
using Nude.API.Contracts.Tags.Responses;

namespace Nude.API.Contracts.Manga.Responses;

public struct MangaResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ExternalMetaResponse? External { get; set; }

    public List<TagResponse> Tags { get; set; }
    public List<FormatResponse> Formats { get; set; }
    public List<string> Images { get; set; }
}