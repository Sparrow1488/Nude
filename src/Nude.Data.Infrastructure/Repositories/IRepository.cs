namespace Nude.Data.Infrastructure.Repositories;

public interface IRepository<TEntity>
{
    Task<TEntity> AddAsync(TEntity entity);
    Task SaveAsync();
}