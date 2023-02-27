using Microsoft.EntityFrameworkCore;
using Nude.Models.Mangas;

namespace Nude.API.Data.Contexts;

public sealed class BotDbContext : DbContext
{
    public BotDbContext(DbContextOptions options) : base(options)
    {
        TghMangas = Set<TghManga>();
    }

    public DbSet<TghManga> TghMangas { get; }
}