using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Users;

namespace Nude.Data.Infrastructure.Extensions;

public static class UserExtensions
{
    public static IQueryable<User> IncludeDependencies(this DbSet<User> dbSet)
    {
        return dbSet.Include(x => x.Accounts);
    }
}