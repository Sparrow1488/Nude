using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nude.Bot.Tg.Services.Messages.Store;

public class MessageItem
{
    public MessageItem(string text, ParseMode parseMode, IReplyMarkup? keyboard=null)
    {
        Keyboard = keyboard;
        Text = text;
        ParseMode = parseMode;
    }

    public IReplyMarkup? Keyboard { get; }
    public string Text { get; }
    public ParseMode ParseMode { get; }
}