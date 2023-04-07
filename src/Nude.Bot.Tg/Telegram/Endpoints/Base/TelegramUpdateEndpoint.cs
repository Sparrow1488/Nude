using Microsoft.Extensions.DependencyInjection;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Telegram.Endpoints.Base;

public abstract class TelegramUpdateEndpoint : TelegramEndpoint
{
    #region Current update scope

    public global::Telegram.Bot.Types.Update Update { get; set; } = null!;
    private Message Message => Update.Message ?? throw new Exception("Tg message not available");
    protected string MessageText => Message?.Text ?? "";
    protected long ChatId => Message.Chat.Id;

    #endregion

    #region Helpers

    protected IMessagesStore MessagesStore => ServiceProvider.GetRequiredService<IMessagesStore>();

    #endregion
    
    protected Task<Message> MessageAsync(string message, ParseMode parseMode = ParseMode.Html)
    {
        return BotClient.SendTextMessageAsync(ChatId, message, parseMode);
    }
    
    protected Task<Message> MessageAsync(MessageItem message)
    {
        return BotUtils.MessageAsync(BotClient, ChatId, message);
    }
}