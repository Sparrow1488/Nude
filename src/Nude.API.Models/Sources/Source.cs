using Nude.API.Models.Abstractions;
using Nude.API.Models.Mangas;

namespace Nude.API.Models.Sources;

public class Source : IEntity
{
    public int Id { get; set; }
    public SourceType Type { get; set; }
    public ICollection<Manga> Mangas { get; set; }
}