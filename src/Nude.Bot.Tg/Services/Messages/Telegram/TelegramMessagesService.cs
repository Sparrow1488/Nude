using Microsoft.EntityFrameworkCore;
using Nude.Data.Infrastructure.Contexts;
using Nude.Models.Messages.Telegram;

namespace Nude.Bot.Tg.Services.Messages.Telegram;

public class TelegramMessagesService : ITelegramMessagesService
{
    private readonly BotDbContext _context;

    public TelegramMessagesService(BotDbContext context)
    {
        _context = context;
    }

    public Task<TelegramConvertingMessage?> GetByTicketIdAsync(int convertTicketId)
    {
        return _context.ConvertingMessages
            .FirstOrDefaultAsync(x => x.ConvertTicketId == convertTicketId);
    }

    public async Task<IEnumerable<TelegramConvertingMessage>> GetSimilarByTicketIdAsync(int convertTicketId)
    {
        return await _context.ConvertingMessages
            .Where(x => x.ConvertTicketId == convertTicketId)
            .ToListAsync();
    }

    public async Task<TelegramConvertingMessage> CreateMessageAsync(TelegramConvertingMessage message)
    {
        await _context.AddAsync(message);
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task DeleteMessageAsync(TelegramConvertingMessage message)
    {
        var existsMessage = await _context.ConvertingMessages
            .FirstOrDefaultAsync(x => x.Id == message.Id);

        if (existsMessage != null)
        {
            _context.Remove(existsMessage);
            await _context.SaveChangesAsync();
        }
    }
}