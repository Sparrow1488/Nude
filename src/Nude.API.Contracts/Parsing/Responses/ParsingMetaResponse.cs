using Nude.Models.Tickets.Parsing;

namespace Nude.API.Contracts.Parsing.Responses;

public struct ParsingMetaResponse
{
    public string SourceItemId { get; set; }
    public string SourceUrl { get; set; }
    public ParsingEntityType EntityType { get; set; }
}