using Nude.Models.Abstractions;
using Nude.Models.Mangas;

namespace Nude.Models.Tags;

public class Tag : IEntity
{
    public int Id { get; set; }
    public string Value { get; set; }
    public string NormalizeValue { get; set; }
    public ICollection<Manga> Mangas { get; set; }
}