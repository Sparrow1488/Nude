using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Nude.API.Contracts.Blacklists.Responses;
using Nude.API.Contracts.Errors.Responses;
using Nude.API.Infrastructure.Extensions;
using Nude.API.Models.Blacklists;
using Nude.API.Models.Users;
using Nude.Bot.Tg.Services.Keyboards;
using Nude.Bot.Tg.Services.Utils;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Services.Messages.Store;

public class MessageStore : IMessagesStore
{
    private readonly Dictionary<string, string> _messages = new();
    private readonly IConfiguration _configuration;

    public MessageStore(IConfiguration configuration)
    {
        _configuration = configuration;
       LoadMessages();
    }

    public Task<MessageItem> GetTicketStatusMessageAsync(
        string time,
        string reqStatus,
        string stage,
        string loaded, 
        string url)
    {
        string text = _messages["ticketinfo"];
        text = string.Format(text, time, reqStatus, stage, loaded, url);
        return Task.FromResult(new MessageItem(text, ParseMode.MarkdownV2));
    }

    public Task<MessageItem> GetReadMangaMessageAsync(string manga)
    {
        var text = $"[Читать онлине]({manga})";
        return Task.FromResult(new MessageItem(text, ParseMode.MarkdownV2));
    }

    public Task<MessageItem> GetStartMessageAsync()
    {
        var startText = _messages["start"];
        return Task.FromResult(new MessageItem(startText, ParseMode.MarkdownV2, KeyboardsStore.MainKeyboard));
    }

    public Task<MessageItem> GetCallbackFailedMessageAsync()
    {
        var text = _messages["callbackfailed"];
        return Task.FromResult(new MessageItem(text, ParseMode.MarkdownV2));
    }

    public Task<MessageItem> GetHelpMessageAsync()
    {
        var startText = _messages["help"];
        return Task.FromResult(new MessageItem(startText, ParseMode.MarkdownV2));
    }

    public Task<MessageItem> GetErrorResponseMessageAsync(ErrorResponse errorResponse)
    {
        const string description = "description";
        if (errorResponse.Data?.Contains(description) ?? false)
        {
            var text = errorResponse.Data[description]!.ToString()!;
            return Task.FromResult(new MessageItem(text, ParseMode.Html));
        }

        return Task.FromResult(new MessageItem(
            errorResponse.Status + ": " + errorResponse.Message,
            ParseMode.Html
        ));
    }

    public Task<MessageItem> GetSourcesMessageAsync(List<string> sources)
    {
        var formatted = sources
            .Select(x => $"`{x}`"
                .Replace("-", "\\-")
                .Replace(".", "\\."))
            .ToArray();
        var text = string.Join("\n", formatted);
        return Task.FromResult(new MessageItem(text, ParseMode.MarkdownV2));
    }

    public Task<MessageItem> GetProfileChapterMessageAsync(
        TelegramUser user, 
        ClaimsIdentity identity,
        string profileCompliment = "невозмутимый")
    {
        var text = string.Format(
            _messages["profile"],
            profileCompliment,
            user.Username,
            RoleUtils.GoodPrintRoleName(identity.GetRoleRequired())
        );
        return Task.FromResult(new MessageItem(text, ParseMode.MarkdownV2, KeyboardsStore.ProfileKeyboard));
    }
    
    public Task<MessageItem> GetImagesUploadMessageAsync(int currentImage, int totalImages)
    {
        return Task.FromResult(new MessageItem(
            $"Загружаем украденную мангу в Telegraph ({currentImage}/{totalImages})",
            ParseMode.Html
        ));
    }

    public Task<MessageItem> GetBlacklistTagsMessageAsync(BlacklistResponse blacklist)
    {
        var text = "У вас нет тегов в списке";
        if (blacklist.Tags.Any())
        {
            text = "Теги в черном списке: " + 
                string.Join(" ", blacklist.Tags.Select(x => $"`{x.Value}`"));
        }

        text += "\n\n" + _messages["blacklisthelp"];

        return Task.FromResult(new MessageItem(text, ParseMode.MarkdownV2, KeyboardsStore.BlacklistKeyboard));
    }

    private void LoadMessages()
    {
        const string filter = "*.md";

        var messagesPath = _configuration["Resources:Path"] + "/Messages";
        var paths = Directory.GetFiles(messagesPath,filter);
        foreach (var path in paths)
        {
            var fileName = Path.GetFileName(path);
            
            var key = fileName.Replace(".md","");
            var message = GetFileText(path);
            _messages.Add(key.ToLower(), message);
        }
    }
    
    private static string GetFileText(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Not found message file on path '{path}'");
        
        return File.ReadAllText(path);
    }

    public MessageItem GetPicturesByTagsMessage()
    {
        var message = _messages["picbytaginfo"];
        return new MessageItem(message, ParseMode.Markdown);
    }
}