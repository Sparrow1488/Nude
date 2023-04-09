using Nude.Models.Mangas;

namespace Nude.Bot.Tg.Services.Messages.Store;

public interface IMessagesStore
{
    Task<MessageItem> GetTicketStatusMessageAsync(string time,
        string reqStatus,
        string stage,
        string loaded, 
        string url);
    Task<MessageItem> GetTghMessageAsync(string manga);
    Task<MessageItem> GetStartMessageAsync();
    Task<MessageItem> GetCallbackFailedMessageAsync();
    Task<MessageItem> GetMenuMessageAsync();
    Task<MessageItem> GetSourcesMessageAsync(List<string> sources);
    Task<MessageItem> GetImagesUploadMessageAsync(int currentImage, int totalImages);
}