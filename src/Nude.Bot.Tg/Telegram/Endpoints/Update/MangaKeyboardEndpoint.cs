﻿using Nude.Bot.Tg.Services.Keyboards;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update
{
    public class MangaKeyboardEndpoint : TelegramUpdateCommandEndpoint
    {
        public MangaKeyboardEndpoint() : base("/manga")
        {
            
        }
        public override async Task HandleAsync(Message message)
        {
            MessageItem messageItem = new MessageItem("Вы перешли в раздел манги",ParseMode.MarkdownV2,BotKeyboardService.MangaKeyboard);
            await MessageAsync(messageItem);
        }
    }
}
