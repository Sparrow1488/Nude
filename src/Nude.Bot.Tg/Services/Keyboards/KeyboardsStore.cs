using Nude.Bot.Tg.Constants;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nude.Bot.Tg.Services.Keyboards;

public static class KeyboardsStore
{
    public static readonly IReplyMarkup MainKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new KeyboardButton[] { NavigationDefaults.PicturesChapter, NavigationDefaults.MangaChapter },
    })
    {
        ResizeKeyboard = true
    };

    public static readonly IReplyMarkup MangaKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new KeyboardButton[] { NavigationDefaults.RandomManga },
        new KeyboardButton[] { NavigationDefaults.HomeChapter }
    })
    {
        ResizeKeyboard = true
    };

    public static readonly IReplyMarkup PictureKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new KeyboardButton[] { NavigationDefaults.RandomPicture },
        new KeyboardButton[] { "/pictag" },
        new KeyboardButton[] { NavigationDefaults.HomeChapter }
    })
    {
        ResizeKeyboard = true
    };
}