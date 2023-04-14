using Nude.API.Models.Abstractions;
using Nude.API.Models.Collections;
using Nude.API.Models.Mangas;

namespace Nude.API.Models.Formats;

public abstract class Format : IEntity
{
    public int Id { get; set; }
    public virtual FormatType Type { get; set; }

    public MangaEntry? MangaEntry { get; set; } = null!;
    public int? MangaEntryId { get; set; }
    public ImageCollection? ImageCollection { get; set; } = null!;
    public int? ImageCollectionId { get; set; }
}