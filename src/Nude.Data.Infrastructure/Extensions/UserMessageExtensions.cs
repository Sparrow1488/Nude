using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Messages;

namespace Nude.Data.Infrastructure.Extensions;

public static class UserMessageExtensions
{
    public static IQueryable<UserMessage> IncludeDependencies(this DbSet<UserMessage> dbSet)
    {
        return dbSet.Include(x => x.Details);
    }
}