using System.Security.Claims;
using Nude.API.Contracts.Blacklists.Responses;
using Nude.API.Contracts.Errors.Responses;
using Nude.API.Contracts.Tags.Responses;
using Nude.API.Models.Users;

namespace Nude.Bot.Tg.Services.Messages.Store;

public interface IMessagesStore
{
    Task<MessageItem> GetTicketStatusMessageAsync(
        string time,
        string reqStatus,
        string stage,
        string loaded, 
        string url
    );
    
    Task<MessageItem> GetReadMangaMessageAsync(string manga);
    Task<MessageItem> GetStartMessageAsync();
    Task<MessageItem> GetCallbackFailedMessageAsync();
    Task<MessageItem> GetHelpMessageAsync();
    Task<MessageItem> GetErrorResponseMessageAsync(ErrorResponse errorResponse);
    Task<MessageItem> GetSourcesMessageAsync(List<string> sources);
    Task<MessageItem> GetProfileChapterMessageAsync(
        TelegramUser user, 
        ClaimsIdentity identity,
        string profileCompliment = "невозмутимый"
    );
    Task<MessageItem> GetImagesUploadMessageAsync(int currentImage, int totalImages);
    Task<MessageItem> GetBlacklistChapterMessageAsync(BlacklistResponse blacklist);
    Task<MessageItem> GetBlacklistTagsMessageAsync(TagResponse[] tags);

}