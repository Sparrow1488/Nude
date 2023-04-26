using Nude.API.Models.Abstractions;
using Nude.API.Models.Formats;

namespace Nude.API.Models.Collections;

public class ImageCollection : IEntity, IFormatContentEntry
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string ContentKey { get; set; } = null!;
    public ICollection<Format> Formats { get; set; } = null!;
    public ICollection<CollectionImage> Images { get; set; } = null!;
    // TODO: add IsSealed property (collection may update in feature)
}