using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Messages;

namespace Nude.Data.Infrastructure.Contexts;

public class FixedBotDbContext : DatabaseContext
{
    public FixedBotDbContext(DbContextOptions<FixedBotDbContext> options) : base(options) { }

    public DbSet<UserMessages> Messages => Set<UserMessages>();
}