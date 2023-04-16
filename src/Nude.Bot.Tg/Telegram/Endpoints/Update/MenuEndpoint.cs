using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot.Types;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class MenuEndpoint : TelegramUpdateCommandEndpoint
{
    public MenuEndpoint() : base("/menu") { }
    
    public override async Task HandleAsync(Message message) =>
        await MessageAsync(await MessagesStore.GetMenuMessageAsync());
}