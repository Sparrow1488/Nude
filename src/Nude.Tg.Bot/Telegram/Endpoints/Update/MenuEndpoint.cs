using Nude.Tg.Bot.Telegram.Endpoints.Base;

namespace Nude.Tg.Bot.Telegram.Endpoints.Update;

public class MenuEndpoint : TelegramUpdateCommandEndpoint
{
    public MenuEndpoint() : base("/menu") { }
    
    public override async Task HandleAsync() =>
        await MessageAsync(await MessagesStore.GetMenuMessageAsync());
}