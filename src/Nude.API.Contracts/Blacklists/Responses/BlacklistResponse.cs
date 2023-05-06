using Nude.API.Contracts.Tags.Responses;

namespace Nude.API.Contracts.Blacklists.Responses;

public struct BlacklistResponse
{
    public int Id { get; set; }
    public TagResponse[] Tags { get; set; }
    public int UserId { get; set; }
}