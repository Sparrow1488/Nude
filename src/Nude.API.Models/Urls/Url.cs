using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Urls;

public class Url : IEntity
{
    public int Id { get; set; }
    public string Value { get; set; }
    public UrlType Type { get; set; } // TODO: can be removed
}