using System.ComponentModel.DataAnnotations;
using Nude.Models.Abstractions;

namespace Nude.Models.Tickets.Parsing;

public sealed class ParsingTicket : IEntity<int>
{
    [Key]
    public int Id { get; set; }
    public ParsingMeta Meta { get; set; }
    public ParsingResult Result { get; set; }
    public ICollection<Subscriber> Subscribers { get; set; }
    public ParsingStatus Status { get; set; }
}