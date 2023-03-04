using Microsoft.Extensions.Configuration;

namespace Nude.Tg.Bot.Services.Messages;

public class MessageStore : IMessagesStore
{
    private readonly IConfiguration _configuration;

    public MessageStore(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public Task<string> GetStartMessageAsync()
    {
        return GetFileMessageTextAsync("/Start.md");
    }

    public async Task<string> GetMenuMessageAsync()
    {
        var startText = await GetStartMessageAsync();
        var text = string.Join("", startText.SkipWhile(x => x != '`')); // first command prefix
        return text;
    }

    private async Task<string> GetFileMessageTextAsync(string name)
    {
        var messagesPath = _configuration["Resources:Path"] + "/Messages";
        var startTextFile = await File.ReadAllTextAsync(messagesPath + name);
        return startTextFile;
    }
}