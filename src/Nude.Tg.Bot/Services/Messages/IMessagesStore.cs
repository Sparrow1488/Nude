namespace Nude.Tg.Bot.Services.Messages;

public interface IMessagesStore
{
    Task<string> GetStartMessageAsync();
    Task<string> GetMenuMessageAsync();
}