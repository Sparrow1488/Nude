using Nude.API.Contracts.Images.Responses;
using Nude.Bot.Tg.Attributes;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Pictures;

[IgnoreEndpoint]
public class PictagEndpoint : TelegramUpdateCommandEndpoint
{
    private readonly INudeClient _nudeClient;
    
    public PictagEndpoint(INudeClient nudeClient) : base("/pictag")
    {
        _nudeClient = nudeClient;
    }
    
    public override async Task HandleAsync()
    {
        var tags = MessageText.Split(' ')
            .Skip(1)
            .Select(x => x.Trim())
            .ToList();

        if (!tags.Any())
        {
            tags.Add("sex");
            tags.Add("highres");
            tags.Add("cum");
            tags.Add("nude");
            tags.Add("1girl");
            tags.Add("striped");
            tags.Add("breasts");
        }
        
        var result = await _nudeClient.FindImagesByTagsAsync(tags);
        if (result.IsSuccess)
        {
            await RetrySendingAsync(() =>
                SendImageResponseAsync(result.ResultValue)
            );
        }
        else
        {
            await MessageAsync(result.Status + ": " + result.Message);
        }
    }

    private async Task SendImageResponseAsync(ImageResponse[] images)
    {
        if (images.Length == 0)
        {
            await MessageAsync("По запросу не найдено");
            return;
        }

        var media = images
            .Select(x => new InputMediaPhoto(x.Url))
            .ToList();

        await BotClient.SendMediaGroupAsync(
            chatId: new ChatId(ChatId),
            media: media
        );
    }

    private static async Task RetrySendingAsync(Func<Task> retry)
    {
        var currentAttempt = 1;

        const int attempts = 3;
        while (currentAttempt <= attempts)
        {
            try
            {
                await retry.Invoke();
                currentAttempt = attempts + 1;
            }
            catch (Exception ex)
            {
                currentAttempt++;
            }
        }
    }
}