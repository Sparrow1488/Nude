using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Tickets;

namespace Nude.Data.Infrastructure.Extensions;

public static class ContentFormatTicketExtensions
{
    public static IQueryable<ContentFormatTicket> IncludeDependencies(
        this DbSet<ContentFormatTicket> queryable)
    {
        return queryable
            .Include(x => x.Context)
            .Include(x => x.Result);
    }
}