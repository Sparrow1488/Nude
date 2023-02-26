using Nude.Models.Abstractions;

namespace Nude.Models.Tags;

public class Tag : IEntity
{
    public int Id { get; set; }
    public string Value { get; set; }
    public string NormalizeValue { get; set; }
}