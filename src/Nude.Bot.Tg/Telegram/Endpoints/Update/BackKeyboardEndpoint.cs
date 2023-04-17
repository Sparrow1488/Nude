using Nude.Bot.Tg.Services.Keyboards;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class BackKeyboardEndpoint : TelegramUpdateCommandEndpoint
{
    public BackKeyboardEndpoint():base("/back")
    {
            
    }
    
    public override async Task HandleAsync(Message message)
    {
        var messageItem = new MessageItem("Вы перешли в главное меню", ParseMode.MarkdownV2, BotKeyboardService.MainKeyboard);
        await MessageAsync(messageItem);
    }
}