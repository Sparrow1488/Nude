using Microsoft.EntityFrameworkCore;
using Nude.Data.Infrastructure.Contexts;
using Nude.Models.Mangas;

namespace Nude.Bot.Tg.Services.Manga;

public class TelegraphMangaService : ITelegraphMangaService
{
    private readonly BotDbContext _context;

    public TelegraphMangaService(BotDbContext context)
    {
        _context = context;
    }
    
    public Task<TghManga?> GetByExternalIdAsync(string externalId)
    {
        return _context.TghMangas.FirstOrDefaultAsync(x => x.ExternalId == externalId);
    }

    public async Task<TghManga?> GetRandomAsync()
    {
        var ids = await _context.TghMangas
            .Select(x => x.Id)
            .ToListAsync();
        
        var randomIndex = Random.Shared.Next(0, ids.Count - 1);
        var randomId = ids[randomIndex];
        
        return await _context.TghMangas.FirstOrDefaultAsync(x => x.Id == randomId);
    }
}