using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Constants;
using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Pictures;

public class PicturesRandomEndpoint : TelegramUpdateCommandEndpoint
{
    private readonly INudeClient _client;
    
    public PicturesRandomEndpoint(INudeClient client) : base(NavigationDefaults.RandomPicture)
    {
        _client = client;
    }

    public override async Task HandleAsync()
    {
        
    }
}