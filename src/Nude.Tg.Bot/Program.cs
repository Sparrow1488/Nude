using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nude.Tg.Bot;
using Nude.Tg.Bot.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;

var nudeBotContext = NudeBotContext.CreateDefault();
var services = nudeBotContext.Services;

var bot = services.GetRequiredService<TelegramBotClient>();
var botInfo = await bot.GetMeAsync();

var logger = services.GetRequiredService<ILogger<Program>>();
logger.LogInformation(botInfo.FirstName + " started");

var handler = services.GetRequiredService<ITelegramHandler>();

bot.StartReceiving(
    handler.HandleUpdateAsync, 
    handler.HandleErrorAsync, 
    new ReceiverOptions()
);

while (true) {  }