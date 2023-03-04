using Nude.Models.Mangas;

namespace Nude.Tg.Bot.Services.Messages;

public interface IMessagesStore
{
    Task<MessageItem> GetTghMessageAsync(TghManga manga);
    Task<MessageItem> GetStartMessageAsync();
    Task<MessageItem> GetMenuMessageAsync();
    Task<MessageItem> GetSourcesMessageAsync(List<string> sources);
}