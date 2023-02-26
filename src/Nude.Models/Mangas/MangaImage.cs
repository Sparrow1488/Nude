using Nude.Models.Abstractions;
using Nude.Models.Urls;

namespace Nude.Models.Mangas;

public class MangaImage : IEntity
{
    public int Id { get; set; }
    public Url Url { get; set; }
    public Manga Manga { get; set; }
}