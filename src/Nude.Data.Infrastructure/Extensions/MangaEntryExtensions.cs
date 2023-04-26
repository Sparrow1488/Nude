using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Mangas;

namespace Nude.Data.Infrastructure.Extensions;

public static class MangaEntryExtensions
{
    public static IQueryable<MangaEntry> IncludeDependencies(this DbSet<MangaEntry> dbSet)
    {
        return dbSet
            .Include(x => x.Formats)
            .Include(x => x.Tags)
            .Include(x => x.Images)
            .ThenInclude(x => x.Url)
            .Include(x => x.ExternalMeta);
    }
}