using Nude.Tg.Bot.Telegram.Endpoints.Base;

namespace Nude.Tg.Bot.Telegram.Endpoints.Update;

public class StartEndpoint : TelegramUpdateCommandEndpoint
{
    public StartEndpoint() : base("/start") { }
    
    public override async Task HandleAsync() =>
        await MessageAsync(await MessagesStore.GetStartMessageAsync());
}