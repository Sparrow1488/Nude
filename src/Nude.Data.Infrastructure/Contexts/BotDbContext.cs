using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Messages;

namespace Nude.Data.Infrastructure.Contexts;

public class BotDbContext : DatabaseContext
{
    public BotDbContext(DbContextOptions<BotDbContext> options) : base(options) { }

    public DbSet<UserMessages> Messages => Set<UserMessages>();
}