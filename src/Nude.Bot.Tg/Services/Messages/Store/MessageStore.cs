using Microsoft.Extensions.Configuration;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Services.Messages.Store;

public class MessageStore : IMessagesStore
{
    private const string SearchPath = "./Resources/Messages"; 
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

    public Task<MessageItem> GetTghMessageAsync(string manga)
    {
        var text = $"[Читать онлине]({manga})";
        return Task.FromResult(new MessageItem(text, ParseMode.MarkdownV2));
    }

    public async Task<MessageItem> GetStartMessageAsync()
    {
        var startText = _messages["start"];
        var menuMessage = await GetMenuMessageAsync();
        var text = startText + menuMessage.Text;
        return new MessageItem(text, ParseMode.MarkdownV2);
    }

    public Task<MessageItem> GetCallbackFailedMessageAsync()
    {
        var text = _messages["callbackfailed"];
        return Task.FromResult(new MessageItem(text, ParseMode.MarkdownV2));
    }

    public Task<MessageItem> GetMenuMessageAsync()
    {
        var startText = _messages["menu"];
        return Task.FromResult(new MessageItem(startText, ParseMode.MarkdownV2));
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

    public Task<MessageItem> GetImagesUploadMessageAsync(int currentImage, int totalImages)
    {
        return Task.FromResult(new MessageItem(
            $"Загружаем украденную мангу в Telegraph ({currentImage}/{totalImages})",
            ParseMode.Html
        ));
    }

    private string GetFileMessageTextAsync(string name)
    {
        var messagesPath = _configuration["Resources:Path"] + "/Messages";
        var startTextFile =File.ReadAllText(messagesPath + name);
        return startTextFile;
    }

    private void LoadMessages()
    {
        const string filter = "*.md";

        var paths = Directory.GetFiles(SearchPath,filter);
        foreach (var path in paths)
        {
            var key = path.Replace(".md","");
            var message = GetFileMessageTextAsync(path);
            
            _messages.Add(key.ToLower(),message);
        }
    }
}