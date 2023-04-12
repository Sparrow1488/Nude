using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Models.Mangas.Meta;
using Nude.API.Models.Tags;
using Nude.API.Models.Tickets;
using Nude.API.Models.Urls;

namespace Nude.Data.Infrastructure.Contexts;

public class AppDbContext : DatabaseContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<MangaEntry> Mangas => Set<MangaEntry>();
    public DbSet<ExternalMeta> ExternalMetas => Set<ExternalMeta>();
    
    public DbSet<Url> Urls => Set<Url>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<MangaImage> MangaImages => Set<MangaImage>();
    
    public DbSet<Format> Formats => Set<Format>();
    public DbSet<TelegraphFormat> TelegraphFormats => Set<TelegraphFormat>();
    
    public DbSet<ContentTicket> ContentTickets => Set<ContentTicket>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<MangaEntry>()
            .HasMany(x => x.Formats)
            .WithOne(x => x.MangaEntry)
            .HasForeignKey(x => x.MangaEntryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}