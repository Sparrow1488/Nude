using Nude.Tg.Bot.Services.Messages;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Nude.Tg.Bot.Telegram.Endpoints.Base;

public abstract class TelegramUpdateEndpoint : TelegramEndpoint
{
    public global::Telegram.Bot.Types.Update Update { get; set; }
    private Message Message => Update.Message ?? throw new Exception("Tg message not available");
    protected string MessageText => Message?.Text ?? "";
    protected long ChatId => Message.Chat.Id;
    
    protected Task<Message> MessageAsync(string message, ParseMode parseMode = ParseMode.Html)
    {
        return BotClient.SendTextMessageAsync(ChatId, message, parseMode);
    }
    
    protected Task<Message> MessageAsync(MessageItem message)
    {
        return BotClient.SendTextMessageAsync(ChatId, message.Text, message.ParseMode);
    }
}