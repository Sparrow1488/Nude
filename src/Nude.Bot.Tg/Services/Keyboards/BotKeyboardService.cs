using Telegram.Bot.Types.ReplyMarkups;

namespace Nude.Bot.Tg.Services.Keyboards;

public static class BotKeyboardService
{
    public static readonly IReplyMarkup MainKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new KeyboardButton[] { "/pictures", "/manga" },
    })
    {
        ResizeKeyboard = true
    };

    public static readonly IReplyMarkup MangaKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new KeyboardButton[] {"/search_by_author", "/randomManga" },
        new KeyboardButton[] { "/back" }
    })
    {
        ResizeKeyboard = true
    };

    public static readonly IReplyMarkup PictureKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new KeyboardButton[] { "/search_by_tags", "/randomPic" },
        new KeyboardButton[] { "/back" }
    })
    {
        ResizeKeyboard = true
    };
}