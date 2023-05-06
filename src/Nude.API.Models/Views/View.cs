using Nude.API.Models.Abstractions;
using Nude.API.Models.Collections;
using Nude.API.Models.Images;
using Nude.API.Models.Mangas;
using Nude.API.Models.Users;

namespace Nude.API.Models.Views;

public class View : IEntity
{
    public int Id { get; set; }
    public User User { get; set; }
    public MangaEntry? Manga { get; set; }
    public ImageEntry? Image { get; set; }
    public ImageCollection? ImageCollection { get; set; }
}