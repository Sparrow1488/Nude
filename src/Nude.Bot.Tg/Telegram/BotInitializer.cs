using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nude.Bot.Tg.Telegram.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Nude.Bot.Tg.Telegram;

public class BotInitializer
{
    public static async Task StartReceiveAsync(IServiceProvider services, CancellationToken ctk)
    {
        var bot = services.GetRequiredService<ITelegramBotClient>();
        var botInfo = await bot.GetMeAsync(ctk);

        var logger = services.GetRequiredService<ILogger<BotInitializer>>();
        logger.LogInformation(botInfo.FirstName + " started");

        var handler = services.GetRequiredService<ITelegramHandler>();

        bot.StartReceiving(
            handler.HandleUpdateAsync, 
            handler.HandleErrorAsync, 
            new ReceiverOptions(),
            ctk
        );
    }
}