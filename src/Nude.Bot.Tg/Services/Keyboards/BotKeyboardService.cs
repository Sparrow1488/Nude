using Telegram.Bot.Types.ReplyMarkups;

namespace Nude.Bot.Tg.Services.Keyboards;

public class BotKeyboardService
{
    public BotKeyboardService()
    {
        Keyboard = new ReplyKeyboardMarkup(new KeyboardButton("/start"));
    }
    
    public ReplyKeyboardMarkup Keyboard { get; }
}