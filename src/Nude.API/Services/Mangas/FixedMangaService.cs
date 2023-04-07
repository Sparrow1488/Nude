using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Mangas;
using Nude.Data.Infrastructure.Contexts;

namespace Nude.API.Services.Mangas;

public class FixedMangaService : IFixedMangaService
{
    private readonly FixedAppDbContext _context;

    public FixedMangaService(FixedAppDbContext context)
    {
        _context = context;
    }
    
    public Task<MangaEntry?> GetByIdAsync(int id)
    {
        return _context.Mangas
            .AsQueryable()
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<MangaEntry?> FindBySourceIdAsync(string id)
    {
        return _context.Mangas
            .AsQueryable()
            .IncludeDependencies()
            .FirstOrDefaultAsync(x =>
                x.ExternalMeta != null && x.ExternalMeta.SourceId == id);
    }
}

public static class MangaEntryExtensions
{
    public static IQueryable<MangaEntry> IncludeDependencies(this IQueryable<MangaEntry> queryable)
    {
        return queryable
            .Include(x => x.Formats)
            .Include(x => x.Tags)
            .Include(x => x.Images)
            .ThenInclude(x => x.Url)
            .Include(x => x.ExternalMeta);
    }
}