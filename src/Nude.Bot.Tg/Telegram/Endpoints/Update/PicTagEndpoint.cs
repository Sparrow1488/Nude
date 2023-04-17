using System.Globalization;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class PicTagEndpoint : TelegramUpdateCommandEndpoint
{
    private readonly INudeClient _nudeClient;
    
    public PicTagEndpoint(INudeClient nudeClient) :base("/pictag")
    {
        _nudeClient = nudeClient;
    }
    
    public override Task HandleAsync()
    {
        // List<string> tags;
        // if (message.Text!.Trim() != "/pictag")
        // {
        //     tags = message.Text.Split(" ").ToList();
        //     tags.RemoveAt(0);
        // }
        // else
        // {
        //     tags = new List<string> { "sex" };
        // }
        // var pictures = await _nudeClient.GetRandomPicturesByTagsAsync(tags);
        // var messageItem = new MessageItem("Пиздим картинки", ParseMode.Markdown);
        // await MessageAsync(messageItem);

        return Task.CompletedTask;
    }
}