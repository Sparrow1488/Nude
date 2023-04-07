using Nude.API.Contracts.Manga.Responses;

namespace Nude.API.Services.Manga;

public interface IMangaService
{
    Task<MangaResponse> GetByIdAsync(int id);
    // Task<MangaResponse> GetMetaAsync(int id); // TODO: implement
    Task<MangaResponse> FindBySourceUrlAsync(string url);
    Task<MangaResponse> FindBySourceIdAsync(string id);
}