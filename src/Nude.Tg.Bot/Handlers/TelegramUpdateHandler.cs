using Nude.Tg.Bot.Endpoints;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Tg.Bot.Handlers;

public static class TelegramUpdateHandler
{
    public static async Task HandleAsync(
        ITelegramBotClient botClient, 
        Update update, 
        CancellationToken ctk)
    {
        if(update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var endpoint = new NudeTelegramEndpoint
            {
                Update = update,
                BotClient = botClient
            };
            if (endpoint.CanHandle())
            {
                await endpoint.HandleAsync();
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    update.Message.Chat, 
                    "Неизвестная команда", 
                    cancellationToken: ctk);
            }
        }
    }
}