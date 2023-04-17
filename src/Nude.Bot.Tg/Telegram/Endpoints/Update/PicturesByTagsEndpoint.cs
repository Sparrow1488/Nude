using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot.Types;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class PicturesByTagsEndpoint : TelegramUpdateCommandEndpoint
{
    public PicturesByTagsEndpoint():base("/search_by_tags")
    {
            
    }
    
    public override async Task HandleAsync(Message message)
    {
        await MessageAsync(MessagesStore.GetPicturesByTagsMessage());
    }
}