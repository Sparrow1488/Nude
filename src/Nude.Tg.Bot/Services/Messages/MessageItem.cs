using Telegram.Bot.Types.Enums;

namespace Nude.Tg.Bot.Services.Messages;

public class MessageItem
{
    public MessageItem(string text, ParseMode parseMode)
    {
        Text = text;
        ParseMode = parseMode;
    }
    
    public string Text { get; }
    public ParseMode ParseMode { get; }
}