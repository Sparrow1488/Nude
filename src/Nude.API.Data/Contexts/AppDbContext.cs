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
        Mangas = Set<Manga>();
        MangaImages = Set<MangaImage>();
        Authors = Set<Author>();
        Tags = Set<Tag>();
        Urls = Set<Url>();
        Sources = Set<Source>();
        ParsingTickets = Set<ParsingTicket>();
    }

    public DbSet<Manga> Mangas { get; }
    public DbSet<MangaImage> MangaImages { get; }
    public DbSet<Author> Authors { get; }
    public DbSet<Tag> Tags { get; }
    public DbSet<Url> Urls { get; }
    public DbSet<Source> Sources { get; }
    public DbSet<ParsingTicket> ParsingTickets { get; }

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