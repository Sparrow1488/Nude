using Nude.API.Models.Notifications;

namespace Nude.Bot.Tg.Services.Handlers;

public interface ICallbackHandler
{
    public Task HandleAsync(Notification notification);
}