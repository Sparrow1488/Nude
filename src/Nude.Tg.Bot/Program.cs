using Nude.Tg.Bot.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;

var bot = new TelegramBotClient("5955339704:AAGNvEDwpB2GmrOwFZUJ2QYV6hNxIiD4JIM");
var me = await bot.GetMeAsync();

Console.WriteLine(me.FirstName + " started");

bot.StartReceiving(
    TelegramUpdateHandler.HandleAsync, 
    TelegramErrorHandler.HandleErrorAsync, 
    new ReceiverOptions()
);

while (true) {  }