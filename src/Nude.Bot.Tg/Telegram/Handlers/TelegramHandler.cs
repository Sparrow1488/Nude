using Microsoft.Extensions.Logging;
using Nude.Bot.Tg.Services.Resolvers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Telegram.Handlers;

public class TelegramHandler : ITelegramHandler
{
    private readonly EndpointsResolver _endpointsResolver;
    private readonly ILogger<TelegramHandler> _logger;

    public TelegramHandler(
        EndpointsResolver endpointsResolver,
        ILogger<TelegramHandler> logger)
    {
        _endpointsResolver = endpointsResolver;
        _logger = logger;
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ctk)
    {
        _logger.LogInformation(
            "Get message '{mess}' by user {user}", 
            update?.Message?.Text ?? "no_message",
            update?.Message?.Chat.Username);

        if(update!.Type == UpdateType.Message)
        {
            try
            {
                var endpoint = _endpointsResolver.GetUpdateHandler(update, botClient);
                await endpoint.HandleAsync(update.Message!);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Что-то пошло не так");
                await botClient.SendTextMessageAsync(
                    update.Message!.Chat.Id, 
                    "😓 Упс! Что-то пошло не так", 
                    cancellationToken: ctk);
            }
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ctk)
    {
        _logger.LogError(exception, "Oh shit, I'm so sorry!");
        throw exception;
    }
}