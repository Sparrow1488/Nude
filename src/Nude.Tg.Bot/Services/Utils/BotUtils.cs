using Nude.Tg.Bot.Services.Messages;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Tg.Bot.Services.Utils;

public static class BotUtils
{
    public static Task<Message> MessageAsync(ITelegramBotClient client, long chatId, MessageItem message)
    {
        return client.SendTextMessageAsync(chatId, message.Text, message.ParseMode);
    }
}