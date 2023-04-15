using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class HelpEndpoint : TelegramUpdateCommandEndpoint
{
    public HelpEndpoint() : base("/help") { }
    
    public override async Task HandleAsync() =>
        await MessageAsync(await MessagesStore.GetHelpMessageAsync());
}