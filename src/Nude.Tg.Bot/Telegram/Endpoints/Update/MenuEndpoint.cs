using Microsoft.Extensions.Configuration;
using Nude.Tg.Bot.Services.Messages;
using Nude.Tg.Bot.Telegram.Endpoints.Base;
using Telegram.Bot.Types.Enums;

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
        await MessageAsync(await _messages.GetMenuMessageAsync(), ParseMode.MarkdownV2);
    }

    public override bool CanHandle()
    {
        return MessageText == "/menu";
    }
}