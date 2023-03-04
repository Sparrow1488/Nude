using Nude.Tg.Bot.Services.Messages;
using Nude.Tg.Bot.Telegram.Endpoints.Base;

namespace Nude.Tg.Bot.Telegram.Endpoints.Update;

public class FindEndpoint : TelegramUpdateEndpoint
{
    private readonly IMessagesStore _messages;
    
    private static readonly List<string> Sources = new() { "nude-moon.org" };

    public FindEndpoint(IMessagesStore messages)
    {
        _messages = messages;
    }
    
    public override async Task HandleAsync()
    {
        if (MessageText.Contains("—sources") || MessageText.Contains("-sources"))
        {
            await MessageAsync(await _messages.GetSourcesMessageAsync(Sources));
        }
        else
        {
            await MessageAsync("На данный момент я не могу выполнить эту команду:)");
        }
    }

    public override bool CanHandle()
    {
        return MessageText.StartsWith("/find");
    }
}