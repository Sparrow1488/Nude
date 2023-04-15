using Nude.API.Contracts.Formats.Responses;

namespace Nude.API.Contracts.Collections.Responses;

public struct ImageCollectionResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string ContentKey { get; set; }
    public int Images { get; set; }
    public ICollection<FormatResponse> Formats { get; set; }
}