using Nude.Bot.Tg.Services.Keyboards;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Auxiliary;

public class KeyboardsEndpoint : TelegramUpdateEndpoint
{
    private static readonly string[] Keyboards = { "/back", "/manga", "/pictures" };
    
    public override bool CanHandle() => Keyboards.Contains(MessageText);
    
    public override async Task HandleAsync()
    {
        var message = MessageText switch
        {
            "/back" => GetMessage("Вы перешли в главное меню", KeyboardsStore.MainKeyboard),
            "/manga" => GetMessage("Вы перешли в раздел с мангой", KeyboardsStore.MangaKeyboard),
            "/pictures" => GetMessage("Вы перешли в раздел с картинками", KeyboardsStore.PictureKeyboard),
            _ => throw new ArgumentOutOfRangeException()
        };

        await MessageAsync(message);
    }

    private static MessageItem GetMessage(string text, IReplyMarkup keyboard) => 
        new(text, ParseMode.Html, keyboard);
}