using Telegram.Bot.Types.ReplyMarkups;

namespace Nude.Bot.Tg.Services.Keyboards;

public static class KeyboardsStore
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
        new KeyboardButton[] { "/manga-random" },
        new KeyboardButton[] { "/back" }
    })
    {
        ResizeKeyboard = true
    };

    public static readonly IReplyMarkup PictureKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new KeyboardButton[] { "/pictag" },
        new KeyboardButton[] { "/back" }
    })
    {
        ResizeKeyboard = true
    };
}