using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Blacklists;

namespace Nude.Data.Infrastructure.Extensions;

public static class BlacklistExtensions
{
    public static IQueryable<Blacklist> IncludeDependencies(this DbSet<Blacklist> dbSet)
    {
        return dbSet
            .Include(x => x.User)
            .Include(x => x.Tags);
    }
}