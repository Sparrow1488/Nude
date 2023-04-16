using Nude.API.Infrastructure.Utility;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot.Types;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class FindEndpoint : TelegramUpdateCommandEndpoint
{
    public FindEndpoint() : base("/find") { }

    public override async Task HandleAsync(Message message)
    {
        if (MessageText.Contains("—sources") || MessageText.Contains("-sources"))
        {
            await MessageAsync(await MessagesStore.GetSourcesMessageAsync(ContentAware.Domains.ToList()));
            return;
        }
        
        await MessageAsync("На данный момент я не могу выполнить эту команду:)");
    }
}