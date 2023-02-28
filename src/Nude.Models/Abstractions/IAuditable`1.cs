namespace Nude.Models.Abstractions;

public interface IAuditable<TId> : IEntity<TId>, IAuditableBase
{
    
}