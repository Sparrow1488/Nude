using Microsoft.Extensions.Configuration;
using Nude.Tg.Bot.Constants;
using Nude.Tg.Bot.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var accessToken = configuration[BotDefaults.TelegramAccessTokenSection];

var bot = new TelegramBotClient(accessToken);
var me = await bot.GetMeAsync();

Console.WriteLine(me.FirstName + " started");

bot.StartReceiving(
    TelegramUpdateHandler.HandleAsync, 
    TelegramErrorHandler.HandleErrorAsync, 
    new ReceiverOptions()
);

while (true) {  }