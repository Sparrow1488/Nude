using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Messages;
using Nude.API.Models.Users;

namespace Nude.Data.Infrastructure.Contexts;

public class BotDbContext : DatabaseContext
{
    public BotDbContext(DbContextOptions<BotDbContext> options) : base(options) { }

    public DbSet<TelegramUser> Users => Set<TelegramUser>();
    public DbSet<UserMessage> Messages => Set<UserMessage>();
}