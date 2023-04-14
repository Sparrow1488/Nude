using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nude.Bot.Tg.Services.Resolvers;
using Nude.Bot.Tg.Services.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Telegram.Handlers;

public class TelegramHandler : ITelegramHandler
{
    private readonly IServiceProvider _services;
    private readonly EndpointsResolver _endpointsResolver;
    private readonly ILogger<TelegramHandler> _logger;

    public TelegramHandler(
        IServiceProvider services,
        EndpointsResolver endpointsResolver,
        ILogger<TelegramHandler> logger)
    {
        _services = services;
        _endpointsResolver = endpointsResolver;
        _logger = logger;
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ctk)
    {
        _logger.LogInformation(
            "Get message '{mess}' by user {user}", 
            update?.Message?.Text ?? "no_message",
            update?.Message?.Chat.Username);

        await using var scope = _services.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<IUserManager>();
        
        if(update!.Type == UpdateType.Message)
        {
            try
            {
                var a=await userManager.CreateAsync(1488, "fsf", "fsd");
                
                var user = update.Message!.From!;
                var result = await userManager.GetUserSessionAsync(user.Id, user.Username);

                if (result.IsSuccess)
                {
                    var session = result.Result!;
                    
                    var endpoint = _endpointsResolver.GetUpdateHandler(update, botClient, session);
                    await endpoint.HandleAsync();
                }
                else
                {
                    throw result.Exception!;
                }
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