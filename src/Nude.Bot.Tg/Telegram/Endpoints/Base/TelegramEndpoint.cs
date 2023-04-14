using Nude.Bot.Tg.Services.Users;
using Telegram.Bot;

namespace Nude.Bot.Tg.Telegram.Endpoints.Base;

public abstract class TelegramEndpoint
{
    public ITelegramBotClient BotClient { get; set; } = null!;
    public IServiceProvider ServiceProvider { get; set; } = null!;
    public UserSession UserSession { get; set; } = null!;
    
    public abstract Task HandleAsync();
    public abstract bool CanHandle();
}