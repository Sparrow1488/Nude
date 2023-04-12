using Nude.API.Models.Abstractions;
using Nude.API.Models.Mangas.Meta;
using Nude.API.Models.Tags;

namespace Nude.API.Models.Images;

public class ImageEntry : IEntity, IAuditable, IContentEntry
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public string ContentKey { get; set; } = null!;
    public ExternalMeta? ExternalMeta { get; set; }
    public ICollection<Tag> Tags { get; set; } = null!;
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}