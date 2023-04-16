using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot.Types;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class StartEndpoint : TelegramUpdateCommandEndpoint
{
    public StartEndpoint() : base("/start") { }
    
    public override async Task HandleAsync(Message message) =>
        await MessageAsync(await MessagesStore.GetStartMessageAsync());
}