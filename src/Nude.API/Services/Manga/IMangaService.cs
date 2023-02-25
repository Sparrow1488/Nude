using Nude.API.Contracts.Manga.Responses;

namespace Nude.API.Services.Manga;

public interface IMangaService
{
    Task<MangaResponse> GetByUrlAsync(string url);
}