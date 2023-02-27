using Microsoft.Extensions.Logging;
using Nude.Tg.Bot.Endpoints.Update;
using Nude.Tg.Bot.Resolvers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Tg.Bot.Handlers;

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

        var handleSuccess = false;
        
        if(update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var endpoint = _endpointsResolver.GetUpdateHandler(update, botClient);
            await endpoint.HandleAsync();
            if (endpoint.GetType() != typeof(TelegramDefaultUpdateEndpoint))
            {
                handleSuccess = true;
            }
        }
        
        _logger.LogInformation("Handle success '{handle}'", handleSuccess);
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ctk)
    {
        throw exception;
    }
}