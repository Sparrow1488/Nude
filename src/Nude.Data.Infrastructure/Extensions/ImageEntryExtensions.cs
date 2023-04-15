using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Images;

namespace Nude.Data.Infrastructure.Extensions;

public static class ImageEntryExtensions
{
    public static IQueryable<ImageEntry> IncludeDependencies(this DbSet<ImageEntry> dbSet)
    {
        return dbSet
            .Include(x => x.ExternalMeta)
            .Include(x => x.Tags);
    }
}