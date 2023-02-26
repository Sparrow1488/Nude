using Nude.Models.Abstractions;
using Nude.Models.Mangas;

namespace Nude.Models.Authors;

public class Author : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Manga> Mangas { get; set; }
}