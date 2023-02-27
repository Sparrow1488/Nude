using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nude.Tg.Bot.Clients.Telegraph;
using Nude.Tg.Bot.Endpoints;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Tg.Bot.Handlers;

public static class TelegramUpdateHandler
{
    private static IHost _host = Host.CreateDefaultBuilder().ConfigureServices(services =>
    {
        services.AddSingleton<ITelegraphClient, DefaultTelegraphClient>();
        services.AddSingleton<NudeTelegramEndpoint>();
    }).Build();
    
    public static async Task HandleAsync(
        ITelegramBotClient botClient, 
        Update update, 
        CancellationToken ctk)
    {
        if(update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var endpoint = GetHost().Services.GetRequiredService<NudeTelegramEndpoint>();
            endpoint.Update = update;
            endpoint.BotClient = botClient;
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

    private static IHost GetHost()
    {
        return _host;
    }
}