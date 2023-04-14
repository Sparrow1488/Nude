using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Collections;

namespace Nude.Data.Infrastructure.Extensions;

public static class ImageCollectionExtensions
{
    public static IQueryable<ImageCollection> IncludeDependencies(this DbSet<ImageCollection> dbSet)
    {
        return dbSet
            .Include(x => x.Formats)
            .Include(x => x.Images);
    }
}