using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Models.Mangas.Meta;
using Nude.API.Models.Tags;
using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.Contexts;
using Nude.API.Models.Tickets.Results;
using Nude.API.Models.Tickets.Subscribers;
using Nude.API.Models.Urls;

namespace Nude.Data.Infrastructure.Contexts;

public class FixedAppDbContext : DatabaseContext
{
    public FixedAppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<MangaEntry> Mangas => Set<MangaEntry>();
    public DbSet<MangaExternalMeta> MangaExternalMetas => Set<MangaExternalMeta>();
    
    public DbSet<Url> Urls => Set<Url>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<MangaImage> MangaImages => Set<MangaImage>();
    
    public DbSet<FormattedContent> FormattedContents => Set<FormattedContent>();
    public DbSet<TelegraphContent> TelegraphContents => Set<TelegraphContent>();
    
    public DbSet<ContentTicket> ContentTickets => Set<ContentTicket>();
    public DbSet<ContentResult> ContentResults => Set<ContentResult>();
    public DbSet<ContentTicketContext> TicketContexts => Set<ContentTicketContext>();
    
    public DbSet<ContentFormatTicket> FormatTickets => Set<ContentFormatTicket>();
    
    public DbSet<Subscriber> Subscribers => Set<Subscriber>();

    #region Feature

    // public DbSet<User> Users => Set<User>();
    // public DbSet<Account> Accounts => Set<Account>();
    // public DbSet<TelegramAccount> TelegramAccounts => Set<TelegramAccount>();
    // public DbSet<UserSubscription> UserSubscriptions => Set<UserSubscription>();
    // public DbSet<Subscription> Subscription => Set<Subscription>();

    #endregion
}