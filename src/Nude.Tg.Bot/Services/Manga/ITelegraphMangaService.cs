using Nude.Models.Mangas;

namespace Nude.Tg.Bot.Services.Manga;

public interface ITelegraphMangaService
{
    Task<TghManga?> GetByExternalIdAsync(string externalId);
    Task<TghManga?> GetRandomAsync();
}