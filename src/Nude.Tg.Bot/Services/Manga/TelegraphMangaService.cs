using Microsoft.EntityFrameworkCore;
using Nude.API.Data.Contexts;
using Nude.Models.Mangas;

namespace Nude.Tg.Bot.Services.Manga;

public class TelegraphMangaService : ITelegraphMangaService
{
    private readonly BotDbContext _context;

    public TelegraphMangaService(BotDbContext context)
    {
        _context = context;
    }
    
    public Task<TghManga?> GetByExternalId(string externalId)
    {
        return _context.TghMangas.FirstOrDefaultAsync(x => x.ExternalId == externalId);
    }
}