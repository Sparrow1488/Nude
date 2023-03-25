using Microsoft.EntityFrameworkCore;
using Nude.API.Data.Contexts;
using Nude.Models.Tickets.Converting;

namespace Nude.Tg.Bot.Services.Convert;

public class ConvertTicketsService : IConvertTicketsService
{
    private readonly BotDbContext _context;

    public ConvertTicketsService(BotDbContext context)
    {
        _context = context;
    }
    
    public Task<ConvertingTicket?> GetByParsingIdAsync(int parsingId)
    {
        return _context.ConvertingTickets
            .FirstOrDefaultAsync(x => x.ParsingId == parsingId);
    }

    public async Task<ConvertingTicket> CreateAsync(int parsingId, long chatId, ConvertingStatus status)
    {
        var ticket = new ConvertingTicket
        {
            ChatId = chatId,
            ParsingId = parsingId,
            Status = status
        };
        
        await _context.AddAsync(ticket);
        await _context.SaveChangesAsync();

        return ticket;
    }

    public async Task<IEnumerable<ConvertingTicket>> GetAllByChatIdAsync(long chatId)
    {
        var tickets = await _context.ConvertingTickets
            .Where(x => 
                (x.Status == ConvertingStatus.ConvertWaiting || 
                 x.Status == ConvertingStatus.ParseWaiting) &&
                x.ChatId == chatId)
            .ToListAsync();
        return tickets;
    }
}