using Nude.Models.Abstractions;

namespace Nude.Models.Tickets;

public class ParsingTicket : IEntity
{
    public int Id { get; set; }
    public string UniqueId { get; set; }
    public string? ExternalId { get; set; }
    public string Url { get; set; }
    public string? Message { get; set; }
    public Status Status { get; set; }
}

// TODO: Replace with ParsingTicket
public sealed class NewParsingTicket : IEntity<string>
{
    public string Id { get; set; } // Use as UniqueId / Token / Key
    public ParsingInfo Info { get; set; }
    public ParsingResult Result { get; set; }
    public Status Status { get; set; }
}

public sealed class ParsingResult : IEntity
{
    public int Id { get; set; }
    public string Message { get; set; }
}

public sealed class ParsingInfo : IEntity
{
    public int Id { get; set; }
    public string? ExternalId { get; set; }
    public string Url { get; set; }
}