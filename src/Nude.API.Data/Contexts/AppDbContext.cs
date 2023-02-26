using Microsoft.EntityFrameworkCore;
using Nude.Models.Authors;
using Nude.Models.Mangas;
using Nude.Models.Sources;
using Nude.Models.Tags;
using Nude.Models.Urls;

namespace Nude.API.Data.Contexts;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        Mangas = Set<Manga>();
        MangaImages = Set<MangaImage>();
        Authors = Set<Author>();
        Tags = Set<Tag>();
        Urls = Set<Url>();
        Sources = Set<Source>();
    }

    public DbSet<Manga> Mangas { get; }
    public DbSet<MangaImage> MangaImages { get; }
    public DbSet<Author> Authors { get; }
    public DbSet<Tag> Tags { get; }
    public DbSet<Url> Urls { get; }
    public DbSet<Source> Sources { get; }
}