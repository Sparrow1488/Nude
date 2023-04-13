using System.Diagnostics;
using Nude.Bot.Tg.Services.KeyBoard;
using Nude.Bot.Tg.Services.Messages.Store;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Bot.Tg.Services.Utils;

public static class BotUtils
{
    public static Task<Message> MessageAsync(ITelegramBotClient client, long chatId, MessageItem message)
    {
        return client.SendTextMessageAsync(chatId, message.Text, message.ParseMode);
    }
    
    public static async Task<Message?> EditMessageAsync(
        ITelegramBotClient client, 
        long chatId, 
        int messageId, 
        MessageItem message)
    {
        Message? tgMessage = null;
        try
        {
            tgMessage = await client.EditMessageTextAsync(
                new ChatId(chatId),
                messageId,
                message.Text,
                message.ParseMode
            );
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return tgMessage;
    }
}