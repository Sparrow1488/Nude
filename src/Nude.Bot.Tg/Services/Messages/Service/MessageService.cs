using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Messages;
using Nude.API.Models.Messages.Details;
using Nude.API.Models.Users;
using Nude.Data.Infrastructure.Contexts;

namespace Nude.Bot.Tg.Services.Messages.Service;

public class MessageService : IMessageService
{
    private readonly BotDbContext _context;

    public MessageService(BotDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserMessage> CreateAsync(
        long chatId,
        int messageId,
        MessageDetails details, 
        TelegramUser owner)
    {
        var message = new UserMessage
        {
            ChatId = chatId,
            MessageId = messageId,
            Details = details,
            OwnerId = owner.Id
        };

        await _context.AddAsync(message);
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<UserMessage> UpdateAsync(int id, MessageDetails details)
    {
        var message = await _context.Messages
            .Include(x => x.Details)
            .FirstAsync(x => x.Id == id);
        
        message.Details = details;
        await _context.SaveChangesAsync();

        return message;
    }

    public Task<UserMessage?> FindAsync(long chatId, int messageId)
    {
        return _context.Messages
            .FirstOrDefaultAsync(x => x.ChatId == chatId && x.MessageId == messageId);
    }

    public async Task<UserMessage?> FindAsync(long chatId, TelegramUser owner, string mediaGroupId)
    {
        var messages = await _context.Messages
            .Where(x => 
                x.ChatId == chatId && 
                x.OwnerId == owner.Id && 
                x.Details is MediaGroupDetails)
            .ToListAsync();

        return messages.FirstOrDefault(x =>
            x.Details is MediaGroupDetails details && details.MediaGroupId == mediaGroupId);
    }
}