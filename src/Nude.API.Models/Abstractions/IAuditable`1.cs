namespace Nude.API.Models.Abstractions;

public interface IAuditable<TId> : IEntity<TId>, IAuditableBase
{
    
}