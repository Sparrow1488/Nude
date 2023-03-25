using Nude.Models.Tickets.Parsing;

namespace Nude.API.Contracts.Parsing.Responses;

public struct ParsingResponse
{
    public int Id { get; set; }
    public ParsingMetaResponse Meta { get; set; }
    public ParsingResultResponse Result { get; set; }
    public int Subscribers { get; set; }
    /// <summary>
    /// <see cref="ParsingStatus"/>
    /// </summary>
    public ParsingStatus Status { get; set; }
}