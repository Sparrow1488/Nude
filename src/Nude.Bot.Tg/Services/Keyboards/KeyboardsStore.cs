using Nude.Bot.Tg.Constants;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nude.Bot.Tg.Services.Keyboards;

public static class KeyboardsStore
{
    public static readonly IReplyMarkup MainKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new KeyboardButton[]
        {
            NavigationCommands.PicturesChapter, 
            NavigationCommands.MangaChapter
        },
        new KeyboardButton[]
        {
            NavigationCommands.ProfileChapter
        }
    })
    {
        ResizeKeyboard = true
    };

    public static readonly IReplyMarkup MangaKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new KeyboardButton[] { NavigationCommands.RandomManga },
        new KeyboardButton[] { NavigationCommands.HomeChapter }
    })
    {
        ResizeKeyboard = true
    };

    public static readonly IReplyMarkup PictureKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new KeyboardButton[] { NavigationCommands.RandomPicture },
        new KeyboardButton[] { NavigationCommands.HomeChapter }
    })
    {
        ResizeKeyboard = true
    };
    
    public static readonly IReplyMarkup ProfileKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new KeyboardButton[]
        {
            NavigationCommands.UpdateToken,
            NavigationCommands.Blacklist
        },
        new KeyboardButton[]
        {
            NavigationCommands.HomeChapter
        },
    })
    {
        ResizeKeyboard = true
    };
    
    public static readonly IReplyMarkup BlacklistKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new KeyboardButton[] { NavigationCommands.HomeChapter }
    })
    {
        ResizeKeyboard = true
    };
}