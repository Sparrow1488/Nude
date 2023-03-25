using Microsoft.EntityFrameworkCore;
using Nude.Models.Mangas;
using Nude.Models.Messages.Telegram;
using Nude.Models.Tickets.Converting;

namespace Nude.API.Data.Contexts;

public sealed class BotDbContext : DatabaseContext
{
    public BotDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<TghManga> TghMangas => Set<TghManga>();
    public DbSet<ConvertingTicket> ConvertingTickets => Set<ConvertingTicket>();
    public DbSet<TelegramConvertingMessage> ConvertingMessages => Set<TelegramConvertingMessage>();
}