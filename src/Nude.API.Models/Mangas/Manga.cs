using Nude.API.Models.Abstractions;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas.Meta;
using Nude.API.Models.Tags;

namespace Nude.API.Models.Mangas;

public class Manga : IAuditable
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public MangaMeta? Meta { get; set; }

    public ICollection<Tag> Tags { get; set; }
    public ICollection<MangaImage> Images { get; set; }
    public ICollection<FormattedContent> Formats { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}