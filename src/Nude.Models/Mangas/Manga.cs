using Nude.Models.Abstractions;
using Nude.Models.Authors;
using Nude.Models.Sources;
using Nude.Models.Tags;
using Nude.Models.Urls;

namespace Nude.Models.Mangas;

public class Manga : IAuditable
{
    public int Id { get; set; }
    public string? ExternalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ICollection<Tag> Tags { get; set; }
    public ICollection<MangaImage> Images { get; set; }
    public int Likes { get; set; }
    public Author Author { get; set; }
    public Source Source { get; set; }
    public Url OriginUrl { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}