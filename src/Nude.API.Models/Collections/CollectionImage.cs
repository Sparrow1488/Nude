using Nude.API.Models.Abstractions;
using Nude.API.Models.Images;

namespace Nude.API.Models.Collections;

public class CollectionImage : IEntity
{
    public int Id { get; set; }
    public ImageEntry Entry { get; set; } = null!;
    public ImageCollection Collection { get; set; } = null!;
}