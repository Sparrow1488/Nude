using Nude.API.Models.Abstractions;
using Nude.API.Models.Mangas.Meta;
using Nude.API.Models.Tags;
using Nude.API.Models.Users;
using Nude.API.Models.Views;

namespace Nude.API.Models.Images;

public class ImageEntry : IEntity, IAuditable, IContentEntry, IViewable
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public string ContentKey { get; set; } = null!;
    public ExternalMeta? ExternalMeta { get; set; }
    public ICollection<Tag> Tags { get; set; } = null!;
    public ICollection<View> Views { get; set; } = null!;
    public User? Owner { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}