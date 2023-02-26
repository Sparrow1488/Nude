using Nude.Models.Abstractions;

namespace Nude.Models.Requests;

public class ParsingRequest : IEntity
{
    public int Id { get; set; }
    public string UniqueId { get; set; }
    public string Url { get; set; }
    public Status Status { get; set; }
}