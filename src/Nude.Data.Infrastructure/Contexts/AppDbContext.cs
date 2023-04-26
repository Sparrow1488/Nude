using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Claims;
using Nude.API.Models.Collections;
using Nude.API.Models.Formats;
using Nude.API.Models.Images;
using Nude.API.Models.Mangas;
using Nude.API.Models.Mangas.Meta;
using Nude.API.Models.Tags;
using Nude.API.Models.Tickets;
using Nude.API.Models.Urls;
using Nude.API.Models.Users;
using Nude.API.Models.Users.Accounts;

namespace Nude.Data.Infrastructure.Contexts;

public class AppDbContext : DatabaseContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Format> Formats => Set<Format>();
    public DbSet<TelegraphFormat> TelegraphFormats => Set<TelegraphFormat>();
    
    public DbSet<MangaEntry> Mangas => Set<MangaEntry>();
    public DbSet<ImageEntry> Images => Set<ImageEntry>();
    public DbSet<ImageCollection> ImageCollections => Set<ImageCollection>();
    
    public DbSet<Tag> Tags => Set<Tag>();
    
    public DbSet<ExternalMeta> ExternalMetas => Set<ExternalMeta>();
    public DbSet<Url> Urls => Set<Url>();
    public DbSet<MangaImage> MangaImages => Set<MangaImage>();
    public DbSet<ContentTicket> ContentTickets => Set<ContentTicket>();

    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<ClaimEntry> Claims => Set<ClaimEntry>();
    public DbSet<TelegramAccount> TelegramAccounts => Set<TelegramAccount>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<MangaEntry>()
            .HasMany(x => x.Formats)
            .WithOne(x => x.MangaEntry)
            .HasForeignKey(x => x.MangaEntryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<ImageCollection>()
            .HasMany(x => x.Formats)
            .WithOne(x => x.ImageCollection)
            .HasForeignKey(x => x.ImageCollectionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<ImageEntry>()
            .HasOne(x => x.Owner)
            .WithMany(x => x.Images)
            .OnDelete(DeleteBehavior.SetNull);
    }
}