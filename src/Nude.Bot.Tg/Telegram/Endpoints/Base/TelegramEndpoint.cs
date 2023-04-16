using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Bot.Tg.Telegram.Endpoints.Base;

public abstract class TelegramEndpoint
{
    public ITelegramBotClient BotClient { get; set; } = null!;
    public IServiceProvider ServiceProvider { get; set; } = null!;
    
    public abstract Task HandleAsync(Message message);
    public abstract bool CanHandle();
}