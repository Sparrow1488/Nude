using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Mangas.Meta;

public class ExternalMeta : IEntity
{
    public int Id { get; set; }
    public string? SourceId { get; set; }
    public string? SourceUrl { get; set; }
}