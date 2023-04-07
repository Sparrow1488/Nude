using Microsoft.EntityFrameworkCore;
using Nude.Models.Abstractions;

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