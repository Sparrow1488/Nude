using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nude.Tg.Bot.Telegram.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Nude.Tg.Bot.Telegram;

public class BotInitializer
{
    public static async Task StartReceiveAsync(IServiceProvider services)
    {
        var bot = services.GetRequiredService<ITelegramBotClient>();
        var botInfo = await bot.GetMeAsync();

        var logger = services.GetRequiredService<ILogger<BotInitializer>>();
        logger.LogInformation(botInfo.FirstName + " started");

        var handler = services.GetRequiredService<ITelegramHandler>();

        bot.StartReceiving(
            handler.HandleUpdateAsync, 
            handler.HandleErrorAsync, 
            new ReceiverOptions()
        );
    }
}