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
            new KeyboardButton[] {"/search_by_author", "/randomManga" },
            new KeyboardButton[] { "/back" }
        })
        {
            ResizeKeyboard = true
        };

        public static readonly ReplyKeyboardMarkup PictureKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "/search_by_tags", "/randomPic" },
            new KeyboardButton[] { "/back" }
        })
        {
            ResizeKeyboard = true
        };
    }
}
