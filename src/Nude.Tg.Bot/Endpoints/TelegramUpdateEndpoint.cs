using Telegram.Bot.Types;

namespace Nude.Tg.Bot.Endpoints;

public abstract class TelegramUpdateEndpoint : TelegramEndpoint
{
    public Update Update { get; set; }
}