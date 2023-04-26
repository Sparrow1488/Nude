using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
    private readonly ILogger<TelegramHandler> _logger;
    private readonly JwtSecurityTokenHandler _jwtHandler;

    public TelegramHandler(
        IServiceProvider services,
        ILogger<TelegramHandler> logger)
    {
        _services = services;
        _logger = logger;
        _jwtHandler = new JwtSecurityTokenHandler();
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ctk)
    {
        _logger.LogInformation(
            "Get message '{mess}' by user {user}", 
            update.Message?.Text ?? "no_message",
            update.Message?.Chat.Username);

        await using var scope = _services.CreateAsyncScope();
        var endpointsResolver = scope.ServiceProvider.GetRequiredService<EndpointsResolver>();
        var userManager = scope.ServiceProvider.GetRequiredService<IUserManager>();
        
        if(update.Type == UpdateType.Message)
        {
            try
            {
                var user = update.Message!.From!;
                var result = await userManager.GetUserSessionAsync(
                    user.Id, 
                    user.Username ?? AnonUsername(user)
                );

                if (result.IsSuccess)
                {
                    var session = result.Result!;
                    
                    var token = _jwtHandler.ReadJwtToken(session.User.AccessToken);
                    var identity = new ClaimsIdentity(token.Claims);
                    
                    // TODO: endpoint requirements (check identity.role)
                    var endpoint = endpointsResolver.GetUpdateHandler(update, botClient, session, identity);
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

    private static string AnonUsername(User user) => "User-" + user.Id;

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ctk)
    {
        _logger.LogError(exception, "Oh shit, I'm so sorry!");
        throw exception;
    }
}