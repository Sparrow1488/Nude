using Telegram.Bot.Types;

namespace Nude.Tg.Bot.Endpoints.Base;

public abstract class TelegramUpdateEndpoint : TelegramEndpoint
{
    public Telegram.Bot.Types.Update Update { get; set; }
    protected Message Message => Update.Message ?? throw new Exception("Tg message not available");
    protected string MessageText => Message?.Text ?? "";
    protected long ChatId => Message.Chat.Id;
}