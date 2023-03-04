using Microsoft.Extensions.Configuration;
using Nude.Models.Mangas;
using Telegram.Bot.Types.Enums;

namespace Nude.Tg.Bot.Services.Messages;

public class MessageStore : IMessagesStore
{
    private readonly IConfiguration _configuration;

    public MessageStore(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<MessageItem> GetTghMessageAsync(TghManga manga)
    {
        var text = $"[Читать без регистрации и смс]({manga.TghUrl})";
        return Task.FromResult(new MessageItem(text, ParseMode.MarkdownV2));
    }

    public async Task<MessageItem> GetStartMessageAsync()
    {
        var startText = await GetFileMessageTextAsync("/Start.md");
        var menuMessage = await GetMenuMessageAsync();
        var text = startText + menuMessage.Text;
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

    private async Task<string> GetFileMessageTextAsync(string name)
    {
        var messagesPath = _configuration["Resources:Path"] + "/Messages";
        var startTextFile = await File.ReadAllTextAsync(messagesPath + name);
        return startTextFile;
    }
}