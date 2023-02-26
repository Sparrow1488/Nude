using Nude.Models.Abstractions;

namespace Nude.Models.Urls;

public class Url : IEntity
{
    public int Id { get; set; }
    public string Value { get; set; }
    public UrlType Type { get; set; }
}