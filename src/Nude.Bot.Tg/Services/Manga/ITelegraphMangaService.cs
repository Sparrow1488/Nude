using Nude.Models.Mangas;

namespace Nude.Bot.Tg.Services.Manga;

public interface ITelegraphMangaService
{
    Task<TghManga?> GetByExternalIdAsync(string externalId);
    Task<TghManga?> GetRandomAsync();
}