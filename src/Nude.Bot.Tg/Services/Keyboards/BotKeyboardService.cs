using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nude.Bot.Tg.Services.Keyboards
{
    public static class BotKeyboardService
    {
        public static readonly ReplyKeyboardMarkup MainKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "/pictures", "/manga" },
        })
        {
            ResizeKeyboard = true
        };

        public static readonly ReplyKeyboardMarkup MangaKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "/search_by_name", "/search_by_author" },
            new KeyboardButton[] { "/random", "/back" }
        })
        {
            ResizeKeyboard = true
        };

        public static readonly ReplyKeyboardMarkup PictureKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "/search_by_author", "/search_by_tags" },
            new KeyboardButton[] { "/random", "/compilation" },
            new KeyboardButton[] { "/back" }
        })
        {
            ResizeKeyboard = true
        };
    }
}
