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
    public class PicTagEndpoint : TelegramUpdateCommandEndpoint
    {
        public PicTagEndpoint():base("/pictag")
        {
            
        }
        public override async Task HandleAsync(Message message)
        {
            MessageItem messageItem = new MessageItem(message.Text, ParseMode.Markdown);
            await MessageAsync(messageItem);
        }
    }
}
