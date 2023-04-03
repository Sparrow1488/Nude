using Microsoft.EntityFrameworkCore;
using Nude.Models.Authors;
using Nude.Models.Mangas;
using Nude.Models.Sources;
using Nude.Models.Tags;
using Nude.Models.Tickets.Parsing;
using Nude.Models.Urls;

namespace Nude.API.Data.Contexts;

public sealed class AppDbContext : DatabaseContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Manga> Mangas => Set<Manga>();
    public DbSet<Url> Urls => Set<Url>();
    public DbSet<MangaImage> MangaImages => Set<MangaImage>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Source> Sources => Set<Source>();
    public DbSet<ParsingTicket> ParsingTickets => Set<ParsingTicket>();
    public DbSet<ParsingMeta> ParsingMetas => Set<ParsingMeta>();
    public DbSet<FeedBackInfo> FeedBackInfos => Set<FeedBackInfo>();
    public DbSet<ParsingResult> ParsingResults => Set<ParsingResult>();
    public DbSet<Subscriber> Subscribers => Set<Subscriber>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParsingTicket>()
            .HasOne(x => x.Meta)
            .WithOne(x => x.Ticket)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ParsingTicket>()
            .HasOne(x => x.Result)
            .WithOne(x => x.Ticket)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
}