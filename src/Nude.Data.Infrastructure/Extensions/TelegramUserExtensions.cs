using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Users;

namespace Nude.Data.Infrastructure.Extensions;

public static class TelegramUserExtensions
{
    public static IQueryable<TelegramUser> IncludeDependencies(this DbSet<TelegramUser> dbSet)
    {
        return dbSet.Include(x => x.Messages);
    }
}