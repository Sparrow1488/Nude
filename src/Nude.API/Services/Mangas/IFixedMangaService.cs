using Nude.API.Models.Mangas;

namespace Nude.API.Services.Mangas;

public interface IFixedMangaService
{
    Task<MangaEntry?> GetByIdAsync(int id);
    Task<MangaEntry?> FindBySourceIdAsync(string id);
}