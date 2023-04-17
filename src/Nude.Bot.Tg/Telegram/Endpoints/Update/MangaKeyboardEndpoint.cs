using Nude.Bot.Tg.Services.Keyboards;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class MangaKeyboardEndpoint : TelegramUpdateCommandEndpoint
{
    public MangaKeyboardEndpoint() : base("/manga")
    {
            
    }
    
    public override async Task HandleAsync()
    {
        var messageItem = new MessageItem("Вы перешли в раздел манги", ParseMode.MarkdownV2, BotKeyboardService.MangaKeyboard);
        await MessageAsync(messageItem);
    }
}