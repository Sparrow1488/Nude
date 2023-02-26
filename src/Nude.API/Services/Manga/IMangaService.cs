using Nude.API.Contracts.Manga.Responses;

namespace Nude.API.Services.Manga;

public interface IMangaService
{
    Task<MangaResponse> GetByIdAsync(int id);
    Task<MangaResponse> GetByUrlAsync(string url);
    Task<MangaResponse> GetByExternalIdAsync(string externalId);
}