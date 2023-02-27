using Nude.Tg.Bot.Endpoints.Base;
using Telegram.Bot.Types;

namespace Nude.Tg.Bot.Endpoints.Update;

public abstract class TelegramUpdateEndpoint : TelegramEndpoint
{
    public Telegram.Bot.Types.Update Update { get; set; }
    protected Message Message => Update.Message ?? throw new Exception("Tg message not aviable");
    protected string MessageText => Message?.Text ?? "";
    protected long ChatId => Message.Chat.Id;
}