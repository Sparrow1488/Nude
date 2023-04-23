using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Constants;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Users;
using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Profile;

public class UpdateTokenEndpoint : TelegramUpdateCommandEndpoint
{
    private readonly INudeClient _client;
    private readonly IUserManager _userManager;
    private readonly IMessagesStore _messagesStore;

    public UpdateTokenEndpoint(
        INudeClient client,
        IUserManager userManager,
        IMessagesStore messagesStore) 
    : base(NavigationCommands.UpdateToken)
    {
        _client = client;
        _userManager = userManager;
        _messagesStore = messagesStore;
    }    
    
    public override async Task HandleAsync()
    {
        var apiResult = await _client.AuthorizeAsync(UserSession.User.Username);
        if (apiResult.IsSuccess)
        {
            await _userManager.UpdateTokenAsync(UserSession.User, apiResult.ResultValue.Token);
            await MessageAsync("Токен успешно обновлен!");
        }
        else
        {
            var badMessage = await _messagesStore.GetErrorResponseMessageAsync(apiResult.ErrorValue);
            await MessageAsync(badMessage);
        }
    }
}