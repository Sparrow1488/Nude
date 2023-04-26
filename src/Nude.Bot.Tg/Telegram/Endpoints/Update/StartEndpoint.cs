using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class StartEndpoint : TelegramUpdateCommandEndpoint
{
    public StartEndpoint() : base("/start") { }
    
    public override async Task HandleAsync() =>
        await MessageAsync(await MessagesStore.GetStartMessageAsync());
}