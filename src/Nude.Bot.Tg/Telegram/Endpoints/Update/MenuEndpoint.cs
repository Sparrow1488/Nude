using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class MenuEndpoint : TelegramUpdateCommandEndpoint
{
    public MenuEndpoint() : base("/menu") { }
    
    public override async Task HandleAsync() =>
        await MessageAsync(await MessagesStore.GetMenuMessageAsync());
}