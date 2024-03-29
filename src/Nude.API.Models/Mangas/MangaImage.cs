using Nude.API.Models.Abstractions;
using Nude.API.Models.Urls;

namespace Nude.API.Models.Mangas;

public class MangaImage : IEntity
{
    public int Id { get; set; }
    public Url Url { get; set; } = null!;
    public MangaEntry MangaEntry { get; set; } = null!;
}