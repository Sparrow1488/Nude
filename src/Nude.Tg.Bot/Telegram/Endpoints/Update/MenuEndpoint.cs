using Nude.Tg.Bot.Services.Messages;
using Nude.Tg.Bot.Services.Messages.Store;
using Nude.Tg.Bot.Telegram.Endpoints.Base;

namespace Nude.Tg.Bot.Telegram.Endpoints.Update;

public class MenuEndpoint : TelegramUpdateEndpoint
{
    private readonly IMessagesStore _messages;

    public MenuEndpoint(IMessagesStore messages)
    {
        _messages = messages;
    }
    
    public override async Task HandleAsync()
    {
        await MessageAsync(await _messages.GetMenuMessageAsync());
    }

    public override bool CanHandle()
    {
        return MessageText == "/menu";
    }
}