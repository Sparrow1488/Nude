using Nude.Tg.Bot.Services.Messages;
using Nude.Tg.Bot.Telegram.Endpoints.Base;

namespace Nude.Tg.Bot.Telegram.Endpoints.Update;

public class StartEndpoint : TelegramUpdateEndpoint
{
    private readonly IMessagesStore _messages;

    public StartEndpoint(IMessagesStore messages)
    {
        _messages = messages;
    }
    
    public override async Task HandleAsync()
    {
        await MessageAsync(await _messages.GetStartMessageAsync());
    }

    public override bool CanHandle()
    {
        return MessageText == "/start";
    }
}