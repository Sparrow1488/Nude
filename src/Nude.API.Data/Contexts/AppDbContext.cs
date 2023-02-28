using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nude.Models.Abstractions;
using Nude.Models.Authors;
using Nude.Models.Mangas;
using Nude.Models.Sources;
using Nude.Models.Tags;
using Nude.Models.Tickets;
using Nude.Models.Urls;

namespace Nude.API.Data.Contexts;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Manga> Mangas { get; set; }
    public DbSet<MangaImage> MangaImages { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Url> Urls { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<ParsingTicket> ParsingTickets { get; set; }
    public DbSet<ParsingResult> ParsingResults { get; set; }
    public DbSet<ParsingMeta> ParsingMetas { get; set; }
    public DbSet<Subscriber> Subscribers { get; set; }
    public DbSet<FeedBackInfo> FeedBackInfos { get; set; }

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

    public override Task<int> SaveChangesAsync(CancellationToken ctk = default)
    {
        GetAuditableEntitiesWithState(EntityState.Modified)
            .ForEach(OnUpdateAuditableEntity);
        
        GetAuditableEntitiesWithState(EntityState.Added)
            .ForEach(OnCreateAuditableEntity);
        
        return base.SaveChangesAsync(ctk);
    }

    private static void OnCreateAuditableEntity(IAuditableBase auditable)
    {
        auditable.CreatedAt = DateTimeOffset.UtcNow;
        auditable.UpdatedAt = DateTimeOffset.UtcNow;
    }
    
    private static void OnUpdateAuditableEntity(IAuditableBase auditable) =>
        auditable.UpdatedAt = DateTimeOffset.UtcNow;

    private List<IAuditableBase> GetAuditableEntitiesWithState(EntityState state)
    {
        var entries = ChangeTracker.Entries();
        return entries.Where(x =>
                x.State == state &&
                x.Entity is IAuditableBase)
            .Select(x => x.Entity)
            .Cast<IAuditableBase>()
            .ToList();
    }
}