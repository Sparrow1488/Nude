using Nude.Models.Abstractions;
using Nude.Models.Mangas;

namespace Nude.Models.Sources;

public class Source : IEntity
{
    public int Id { get; set; }
    public SourceType Type { get; set; }
    public ICollection<Manga> Mangas { get; set; }
}