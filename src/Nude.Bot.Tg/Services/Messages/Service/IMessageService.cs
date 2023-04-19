using Nude.API.Models.Messages;
using Nude.API.Models.Messages.Details;
using Nude.API.Models.Users;

namespace Nude.Bot.Tg.Services.Messages.Service;

public interface IMessageService
{
    Task<UserMessage> CreateAsync(
        long chatId, 
        int messageId, 
        MessageDetails details,
        TelegramUser owner
    );

    Task<UserMessage> UpdateAsync(int id, MessageDetails details);
    Task<UserMessage?> FindAsync(long chatId, int messageId);
    Task<UserMessage?> FindAsync(long chatId, TelegramUser owner, string? mediaGroupId);
}