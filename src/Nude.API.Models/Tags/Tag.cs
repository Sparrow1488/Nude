using Nude.API.Models.Abstractions;
using Nude.API.Models.Blacklists;
using Nude.API.Models.Images;
using Nude.API.Models.Mangas;

namespace Nude.API.Models.Tags;

public class Tag : IEntity
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
    public string? NormalizeValue { get; set; }
    public TagType Type { get; set; }
    public ICollection<MangaEntry> Mangas { get; set; } = null!;
    public ICollection<ImageEntry> Images { get; set; } = null!;
    public ICollection<Blacklist> Blacklists { get; set; } = null!;
}