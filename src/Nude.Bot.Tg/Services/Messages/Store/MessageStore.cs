using Microsoft.Extensions.Configuration;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Services.Messages.Store;

public class MessageStore : IMessagesStore
{
    private readonly IConfiguration _configuration;

    public MessageStore(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<MessageItem> GetTicketStatusMessageAsync(
        string time,
        string reqStatus,
        string stage,
        string loaded, 
        string url)
    {
        string text = await GetFileMessageTextAsync("/TicketInfo.md");
        text = string.Format(text, time, reqStatus, stage, loaded, url);
        return new MessageItem(text, ParseMode.MarkdownV2);
    }

    public Task<MessageItem> GetTghMessageAsync(string manga)
    {
        var text = $"[Читать онлине]({manga})";
        return Task.FromResult(new MessageItem(text, ParseMode.MarkdownV2));
    }

    public async Task<MessageItem> GetStartMessageAsync()
    {
        var startText = await GetFileMessageTextAsync("/Start.md");
        var menuMessage = await GetMenuMessageAsync();
        var text = startText + menuMessage.Text;
        return new MessageItem(text, ParseMode.MarkdownV2);
    }

    public async Task<MessageItem> GetCallbackFailedMessageAsync()
    {
        var text = await GetFileMessageTextAsync("/CallbackFailed.md");
        return new MessageItem(text, ParseMode.MarkdownV2);
    }

    public async Task<MessageItem> GetMenuMessageAsync()
    {
        var startText = await GetFileMessageTextAsync("/Menu.md");
        return new MessageItem(startText, ParseMode.MarkdownV2);
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

    private async Task<string> GetFileMessageTextAsync(string name)
    {
        var messagesPath = _configuration["Resources:Path"] + "/Messages";
        var startTextFile = await File.ReadAllTextAsync(messagesPath + name);
        return startTextFile;
    }
}