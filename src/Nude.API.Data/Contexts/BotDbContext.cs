using Microsoft.EntityFrameworkCore;
using Nude.Models.Mangas;
using Nude.Models.Tickets.Converting;

namespace Nude.API.Data.Contexts;

public sealed class BotDbContext : DbContext
{
    public BotDbContext(DbContextOptions options) : base(options)
    {
        TghMangas = Set<TghManga>();
        ConvertingTickets = Set<ConvertingTicket>();
    }

    public DbSet<TghManga> TghMangas { get; }
    public DbSet<ConvertingTicket> ConvertingTickets { get; }
}