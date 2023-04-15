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
        public static readonly InlineKeyboardMarkup TagsKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("sex","sex"),
                InlineKeyboardButton.WithCallbackData("yuri","yuri"),
                InlineKeyboardButton.WithCallbackData("bdsm","bdsm"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("nude","nude"),
                InlineKeyboardButton.WithCallbackData("pussy","pussy"),
                InlineKeyboardButton.WithCallbackData("anal","anal"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("✅Загрузить картинки✅","/load_pic_by_tag"),
            },
        });
        public static readonly ReplyKeyboardMarkup MainKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "/pictures", "/manga" },
        })
        {
            ResizeKeyboard = true
        };

        public static readonly ReplyKeyboardMarkup MangaKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] {"/search_by_author", "/random" },
            new KeyboardButton[] { "/back" }
        })
        {
            ResizeKeyboard = true
        };

        public static readonly ReplyKeyboardMarkup PictureKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "/search_by_tags", "/random" },
            new KeyboardButton[] { "/back" }
        })
        {
            ResizeKeyboard = true
        };
    }
}
