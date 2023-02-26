using Nude.Models.Mangas;
using Nude.Models.Sources;

namespace Nude.API.Data.Repositories;

public interface IMangaRepository : IRepository<Manga>
{
    Task<Manga> AddAsync(
        string externalId,
        string title, 
        string desc, 
        IEnumerable<string> tags, 
        IEnumerable<string> images,
        int likes,
        string authorName,
        SourceType sourceType,
        string originUrl);
}