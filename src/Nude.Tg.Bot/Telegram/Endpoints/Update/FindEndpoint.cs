using Nude.API.Infrastructure.Constants;
using Nude.Tg.Bot.Telegram.Endpoints.Base;

namespace Nude.Tg.Bot.Telegram.Endpoints.Update;

public class FindEndpoint : TelegramUpdateCommandEndpoint
{
    public FindEndpoint() : base("/find") { }

    public override async Task HandleAsync()
    {
        if (MessageText.Contains("—sources") || MessageText.Contains("-sources"))
        {
            await MessageAsync(await MessagesStore.GetSourcesMessageAsync(AvailableSources.Domains.ToList()));
            return;
        }
        
        await MessageAsync("На данный момент я не могу выполнить эту команду:)");
    }
}