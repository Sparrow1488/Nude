using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class PicturesByTagsEndpoint : TelegramUpdateCommandEndpoint
{
    public PicturesByTagsEndpoint() : base("/search_by_tags") { }
    
    public override async Task HandleAsync() =>
        await MessageAsync(MessagesStore.GetPicturesByTagsMessage());
}