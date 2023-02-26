using Nude.Models.Abstractions;
using Nude.Models.Authors;
using Nude.Models.Sources;
using Nude.Models.Tags;
using Nude.Models.Urls;

namespace Nude.Models.Mangas;

public class Manga : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ICollection<Tag> Tags { get; set; }
    public ICollection<MangaImage> Images { get; set; }
    public int Likes { get; set; }
    public Author Author { get; set; }
    public Source Source { get; set; }
    public Url OriginUrl { get; set; }
}