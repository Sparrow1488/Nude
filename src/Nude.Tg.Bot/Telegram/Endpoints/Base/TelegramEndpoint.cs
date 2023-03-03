using Telegram.Bot;

namespace Nude.Tg.Bot.Telegram.Endpoints.Base;

public abstract class TelegramEndpoint
{
    public ITelegramBotClient BotClient { get; set; }
    
    public abstract Task HandleAsync();
    public abstract bool CanHandle();
}