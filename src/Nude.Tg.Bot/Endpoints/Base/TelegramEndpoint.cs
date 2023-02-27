using Telegram.Bot;

namespace Nude.Tg.Bot.Endpoints.Base;

public abstract class TelegramEndpoint
{
    public ITelegramBotClient BotClient { get; set; }
    public NudeBotContext Context { get; set; }
    
    public abstract Task HandleAsync();
    public abstract bool CanHandle();
}