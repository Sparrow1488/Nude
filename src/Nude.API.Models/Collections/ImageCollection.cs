using Nude.API.Models.Abstractions;
using Nude.API.Models.Formats;
using Nude.API.Models.Views;

namespace Nude.API.Models.Collections;

public class ImageCollection : IEntity, IFormatContentEntry, IViewable
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string ContentKey { get; set; } = null!;
    public ICollection<Format> Formats { get; set; } = null!;
    public ICollection<CollectionImageEntry> Images { get; set; } = null!;
    public ICollection<View> Views { get; set; } = null!;

    // TODO: add IsSealed property (collection may update in feature)
}