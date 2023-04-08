using Nude.API.Models.Mangas;
using Nude.API.Services.Mangas.Results;

namespace Nude.API.Services.Mangas;

public interface IMangaService
{
    Task<MangaCreationResult> CreateAsync(
        string title, 
        string description,
        IEnumerable<string> images,
        IEnumerable<string>? tags = null,
        string? author = null,
        string? externalSourceId = null,
        string? externalSourceUrl = null);

    Task<MangaEntry?> GetByIdAsync(int id);
    Task<MangaEntry?> FindBySourceIdAsync(string id);
}