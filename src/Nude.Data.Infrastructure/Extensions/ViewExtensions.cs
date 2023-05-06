using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Views;

namespace Nude.Data.Infrastructure.Extensions;

public static class ViewExtensions
{
    public static IQueryable<View> IncludeDependencies(this DbSet<View> queryable)
    {
        return queryable
            .Include(x => x.User)
            .Include(x => x.Manga)
            .Include(x => x.Image)
            .Include(x => x.ImageCollection);
    }
}