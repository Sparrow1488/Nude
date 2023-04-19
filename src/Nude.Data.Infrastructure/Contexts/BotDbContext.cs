using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Media;
using Nude.API.Models.Messages;
using Nude.API.Models.Messages.Details;
using Nude.API.Models.Users;

namespace Nude.Data.Infrastructure.Contexts;

public class BotDbContext : DatabaseContext
{
    public BotDbContext(DbContextOptions<BotDbContext> options) : base(options) { }

    public DbSet<TelegramUser> Users => Set<TelegramUser>();
    public DbSet<TelegramMedia> Medias => Set<TelegramMedia>();
    public DbSet<UserMessage> Messages => Set<UserMessage>();
    public DbSet<MessageDetails> MessageDetails => Set<MessageDetails>();
    public DbSet<MediaGroupDetails> MediaGroupDetails => Set<MediaGroupDetails>();
    public DbSet<ContentTicketDetails> ContentTicketDetails => Set<ContentTicketDetails>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TelegramUser>()
            .HasMany(x => x.Messages)
            .WithOne(x => x.Owner)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<UserMessage>()
            .HasOne(x => x.Details)
            .WithOne(x => x.Message)
            .HasForeignKey<UserMessage>(x => x.DetailsId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
}