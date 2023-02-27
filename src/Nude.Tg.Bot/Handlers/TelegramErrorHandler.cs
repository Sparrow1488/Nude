using Telegram.Bot;

namespace Nude.Tg.Bot.Handlers;

public static class TelegramErrorHandler
{
    public static Task HandleErrorAsync(
        ITelegramBotClient botClient, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        // Console.WriteLine(JsonConvert.SerializeObject(exception));
        Console.WriteLine("Error! Bot exit");
        return Task.CompletedTask;
    }
}