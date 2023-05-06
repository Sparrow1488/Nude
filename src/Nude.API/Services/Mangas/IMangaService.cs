using Nude.API.Models.Blacklists;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Services.Mangas.Results;

namespace Nude.API.Services.Mangas;

public interface IMangaService
{
    Task<MangaCreationResult> CreateAsync(
        string title, 
        string description,
        string contentKey,
        IEnumerable<string> images,
        IEnumerable<string>? tags = null,
        string? author = null,
        string? externalSourceId = null,
        string? externalSourceUrl = null);

    Task<MangaEntry?> GetByIdAsync(int id);
    Task<int[]> GetAllAsync(Blacklist? blacklist = null);
    Task<MangaEntry?> GetRandomAsync(SearchMangaFilter? filter = null, Blacklist? blacklist = null);
    Task<MangaEntry?> FindBySourceIdAsync(string id);
    Task<MangaEntry?> FindBySourceUrlAsync(string url, FormatType? format = null);
    Task<MangaEntry?> FindByContentKeyAsync(string contentKey, FormatType? format = null);
    Task<MangaEntry> AddFormatAsync(MangaEntry manga, Format format);
}