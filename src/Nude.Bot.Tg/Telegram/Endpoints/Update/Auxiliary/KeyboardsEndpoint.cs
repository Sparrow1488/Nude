using Nude.Bot.Tg.Constants;
using Nude.Bot.Tg.Services.Keyboards;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Auxiliary;

public class KeyboardsEndpoint : TelegramUpdateEndpoint
{
    private readonly IMessagesStore _messagesStore;

    private static readonly string[] Keyboards =
    {
        NavigationCommands.HomeChapter, 
        NavigationCommands.MangaChapter,
        NavigationCommands.PicturesChapter,
        NavigationCommands.ProfileChapter
    };

    public KeyboardsEndpoint(IMessagesStore messagesStore)
    {
        _messagesStore = messagesStore;
    }
    
    public override bool CanHandle() => Keyboards.Contains(MessageText);
    
    public override async Task HandleAsync()
    {
        const string manga = NavigationCommands.MangaChapter;
        const string pictures = NavigationCommands.PicturesChapter;
        const string home = NavigationCommands.HomeChapter;
        const string profile = NavigationCommands.ProfileChapter;
        
        var message = MessageText switch
        {
            home => GetMessage("Вы перешли в главное меню", KeyboardsStore.MainKeyboard),
            manga => GetMessage("Вы перешли в раздел с мангой", KeyboardsStore.MangaKeyboard),
            pictures => GetMessage("Вы перешли в раздел с картинками", KeyboardsStore.PictureKeyboard),
            profile => await _messagesStore.GetProfileChapterMessageAsync(UserSession.User, Identity),
            _ => throw new ArgumentOutOfRangeException()
        };

        await MessageAsync(message);
    }

    private static MessageItem GetMessage(string text, IReplyMarkup keyboard) => 
        new(text, ParseMode.Html, keyboard);
}