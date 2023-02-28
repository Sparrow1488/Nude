using Nude.Models.Mangas;
using Nude.Models.Sources;

namespace Nude.API.Data.Repositories;

public interface IMangaRepository : IRepository<Manga>
{
    Task<Manga> AddAsync(Nude.Models.Manga manga, SourceType sourceType);
}