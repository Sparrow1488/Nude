using Nude.API.Contracts.Metas.Responses;
using Nude.API.Contracts.Tags.Responses;

namespace Nude.API.Contracts.Images.Responses;

public struct ImageResponse
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string ContentKey { get; set; }
    public ExternalMetaResponse External { get; set; }
    public ICollection<TagResponse> Tags { get; set; }
}