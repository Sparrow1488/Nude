using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Tickets;

namespace Nude.Data.Infrastructure.Extensions;

public static class ContentTicketExtensions
{
    public static IQueryable<ContentTicket> IncludeDependencies(
        this DbSet<ContentTicket> dbSet)
    {
        return dbSet
            .Include(x => x.User);
    }
}