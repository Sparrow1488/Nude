namespace Nude.API.Data.Repositories;

public interface IRepository<TEntity>
{
    Task<TEntity> AddAsync(TEntity entity);
    Task SaveAsync();
}