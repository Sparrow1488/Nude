using Nude.API.Models.Formats;
using Nude.API.Models.Tags;

namespace Nude.API.Contracts.Manga.Responses;

public struct MangaResponse
{
    public int Id { get; set; }
    public string ExternalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    public List<string> Images { get; set; }
    public int Likes { get; set; }
    public string Author { get; set; }
}

public struct NewMangaResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public MangaExternalMetaResponse? External { get; set; }

    public List<TagResponse> Tags { get; set; }
    public List<FormattedContentResponse> Formats { get; set; }
    public List<string> Images { get; set; }
}

public struct MangaExternalMetaResponse
{
    public string SourceUrl { get; set; }
    public string SourceId { get; set; }
}

public struct TagResponse
{
    public string Value { get; set; }
    public string NormalizeValue { get; set; }
    public TagType Type { get; set; }
}

public abstract class FormattedContentResponse
{
    public FormatType Type { get; set; }
}

public class TelegraphContentResponse : FormattedContentResponse
{
    public string Url { get; set; }
}