using Nude.Bot.Tg.Services.Keyboards;
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
    public class PicturesByTagsEndpoint : TelegramUpdateCommandEndpoint
    {
        public PicturesByTagsEndpoint():base("/search_by_tags")
        {
            
        }
        public override async Task HandleAsync()
        {
            MessageItem messageItem = new MessageItem("Введите теги", ParseMode.MarkdownV2, BotKeyboardService.TagsKeyboard);
            await MessageAsync(messageItem);
        }
    }
}
