using Nude.API.Models.Abstractions;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas.Meta;
using Nude.API.Models.Tags;
using Nude.API.Models.Views;

namespace Nude.API.Models.Mangas;

public class MangaEntry : IEntity, IAuditable, IFormatContentEntry, IViewable
{
    public int Id { get; set; }
    public string ContentKey { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ExternalMeta? ExternalMeta { get; set; }

    public ICollection<Tag> Tags { get; set; } = null!;
    public ICollection<MangaImage> Images { get; set; } = null!;
    public ICollection<Format> Formats { get; set; } = null!;
    public ICollection<View> Views { get; set; } = null!;
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}