﻿using Nude.Bot.Tg.Services.Keyboards;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update
{
    public class BackKeyboardEndpoint : TelegramUpdateCommandEndpoint
    {
        public BackKeyboardEndpoint():base("/back")
        {
            
        }
        public override async Task HandleAsync()
        {
            MessageItem messageItem = new MessageItem("Вы перешли в главное меню", ParseMode.MarkdownV2, BotKeyboardService.MainKeyboard);
            await MessageAsync(messageItem);
        }
    }
}
