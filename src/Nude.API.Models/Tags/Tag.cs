using Nude.API.Models.Abstractions;
using Nude.API.Models.Mangas;

namespace Nude.API.Models.Tags;

public class Tag : IEntity
{
    public int Id { get; set; }
    public string Value { get; set; }
    public string NormalizeValue { get; set; }
    public TagType Type { get; set; }
    public ICollection<MangaEntry> Mangas { get; set; }
}