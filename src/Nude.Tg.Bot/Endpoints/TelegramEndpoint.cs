using Telegram.Bot;

namespace Nude.Tg.Bot.Endpoints;

// TODO: create from EndpointFactory
public abstract class TelegramEndpoint
{
    public ITelegramBotClient BotClient { get; set; }
    
    public abstract Task HandleAsync();
    public abstract bool CanHandle();
}