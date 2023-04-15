using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Messages;
using Nude.API.Models.Users;

namespace Nude.Data.Infrastructure.Contexts;

public class BotDbContext : DatabaseContext
{
    public BotDbContext(DbContextOptions<BotDbContext> options) : base(options) { }

    public DbSet<TelegramUser> Users => Set<TelegramUser>();
    public DbSet<UserMessage> Messages => Set<UserMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TelegramUser>()
            .HasMany(x => x.Messages)
            .WithOne(x => x.Owner)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
}