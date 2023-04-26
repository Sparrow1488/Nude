using Nude.API.Models.Tags;

namespace Nude.API.Contracts.Tags.Responses;

public struct TagResponse
{
    public string Value { get; set; }
    public string NormalizeValue { get; set; }
    public TagType Type { get; set; }
}