using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Messages;

namespace Nude.Data.Infrastructure.Contexts;

public class FixetBotDbContext : DatabaseContext
{
    public DbSet<UserMessages> Messages => Set<UserMessages>();
}