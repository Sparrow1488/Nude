using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nude.Bot.Tg.Telegram;
using Nude.Bot.Tg.Telegram.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Nude.Bot.Tg.Services.bgServices;

public class BotBgService : BackgroundService
{
    private IServiceProvider _services;

    public BotBgService(IServiceProvider services)
    {
        _services = services;
    }
    protected async override Task ExecuteAsync(CancellationToken ctk)
    {
        var bot = _services.GetRequiredService<ITelegramBotClient>();
        var botInfo = await bot.GetMeAsync(ctk);

        var logger = _services.GetRequiredService<ILogger<BotInitializer>>();
        logger.LogInformation(botInfo.FirstName + " started");

        var handler = _services.GetRequiredService<ITelegramHandler>();

        bot.StartReceiving(
            handler.HandleUpdateAsync, 
            handler.HandleErrorAsync, 
            new ReceiverOptions(),
            ctk
        );
    }
}