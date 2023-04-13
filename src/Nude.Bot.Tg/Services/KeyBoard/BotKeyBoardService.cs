using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nude.Bot.Tg.Services.KeyBoard
{
    public class BotKeyBoardService
    {
        public ReplyKeyboardMarkup KeyBoard { get; private set; }
        public BotKeyBoardService()
        {
            KeyBoard = new ReplyKeyboardMarkup(new KeyboardButton("/start"));
        }
    }
}
