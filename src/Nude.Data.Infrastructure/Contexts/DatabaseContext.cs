using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Abstractions;

namespace Nude.Data.Infrastructure.Contexts;

public abstract class DatabaseContext : DbContext
{
    protected DatabaseContext()
    {
    }

    protected DatabaseContext(DbContextOptions options) : base(options)
    {
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken ctk = default)
    {
        GetAuditableEntitiesWithState(EntityState.Modified)
            .ForEach(OnUpdateAuditableEntity);
        
        GetAuditableEntitiesWithState(EntityState.Added)
            .ForEach(OnCreateAuditableEntity);
        
        return base.SaveChangesAsync(ctk);
    }

    private static void OnCreateAuditableEntity(IAuditable auditable)
    {
        auditable.CreatedAt = DateTimeOffset.UtcNow;
        auditable.UpdatedAt = DateTimeOffset.UtcNow;
    }
    
    private static void OnUpdateAuditableEntity(IAuditable auditable) =>
        auditable.UpdatedAt = DateTimeOffset.UtcNow;

    private List<IAuditable> GetAuditableEntitiesWithState(EntityState state)
    {
        var entries = ChangeTracker.Entries();
        return entries.Where(x =>
                x.State == state &&
                x.Entity is IAuditable)
            .Select(x => x.Entity)
            .Cast<IAuditable>()
            .ToList();
    }
}