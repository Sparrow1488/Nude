using Microsoft.EntityFrameworkCore;
using Nude.Models.Authors;
using Nude.Models.Mangas;
using Nude.Models.Sources;
using Nude.Models.Tags;
using Nude.Models.Tickets.Parsing;
using Nude.Models.Urls;
using Nude.Models.Users;
using Nude.Models.Users.Accounts;
using Nude.Models.Users.Subscriptions;

namespace Nude.API.Data.Contexts;

public sealed class AppDbContext : DatabaseContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Manga> Mangas => Set<Manga>();
    public DbSet<Url> Urls => Set<Url>();
    public DbSet<MangaImage> MangaImages => Set<MangaImage>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Source> Sources => Set<Source>();
    public DbSet<ParsingTicket> ParsingTickets => Set<ParsingTicket>();
    public DbSet<ParsingMeta> ParsingMetas => Set<ParsingMeta>();
    public DbSet<FeedBackInfo> FeedBackInfos => Set<FeedBackInfo>();
    public DbSet<ParsingResult> ParsingResults => Set<ParsingResult>();
    public DbSet<Subscriber> Subscribers => Set<Subscriber>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<TelegramAccount> TelegramAccounts => Set<TelegramAccount>();
    public DbSet<UserSubscription> UserSubscriptions => Set<UserSubscription>();
    public DbSet<Subscription> Subscription => Set<Subscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParsingTicket>()
            .HasOne(x => x.Meta)
            .WithOne(x => x.Ticket)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ParsingTicket>()
            .HasOne(x => x.Result)
            .WithOne(x => x.Ticket)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<User>()
            .HasOne(x => x.UserSubscription)
            .WithOne(x => x.Owner)
            .HasForeignKey<UserSubscription>()
            .OnDelete(DeleteBehavior.Cascade);
        //
        // modelBuilder.Entity<Subscription>()
        //     .HasMany(x => x.UserSubscriptions)
        //     .WithOne(x => x.Subscription)
        //     .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
}