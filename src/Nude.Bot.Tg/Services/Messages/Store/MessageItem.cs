using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nude.Bot.Tg.Services.Messages.Store;

public class MessageItem
{
    public MessageItem(string text, ParseMode parseMode, ReplyKeyboardMarkup? keyboard=null)
    {
        Keyboard = keyboard;
        Text = text;
        ParseMode = parseMode;
    }
    public ReplyKeyboardMarkup? Keyboard { get; }
    public string Text { get; }
    public ParseMode ParseMode { get; }
}