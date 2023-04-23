using Microsoft.EntityFrameworkCore;
using Nude.API.Contracts.Images.Responses;
using Nude.API.Models.Enums;
using Nude.API.Models.Media;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Constants;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Nude.Data.Infrastructure.Contexts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Pictures;

public class PicturesRandomEndpoint : TelegramUpdateCommandEndpoint
{
    private readonly INudeClient _client;
    private readonly BotDbContext _context;
    private List<ImageResponse> _uncachedImages = new();
    private List<InputMedia> _inputMedia = new();

    public PicturesRandomEndpoint(
        INudeClient client,
        BotDbContext context)
    : base(NavigationCommands.RandomPicture)
    {
        _client = client;
        _context = context;
    }

    public override async Task HandleAsync()
    {
        var result = await _client.GetRandomImagesAsync();
        if (result.IsSuccess)
        {
            var cachedMedia = await GetCachedMediaAsync(result.ResultValue);
            _inputMedia = new List<InputMedia>();

            cachedMedia.ToList().ForEach(x => {
                var media = new InputMedia(x.FileId);
                _inputMedia.Add(media);
            });

            using var client = new HttpClient();
            var fileStreamsList = new List<Stream>();
            
            foreach (var image in _uncachedImages)
            {
                var stream = await client.GetStreamAsync(image.Url);
                fileStreamsList.Add(stream);
                _inputMedia.Add(
                    new InputMedia(stream, image.GetHashCode() + "-nude-bot-image.png")        
                );
            }

            await SendMediaAsync();
            fileStreamsList.ForEach(x => x.Close());
        }
        else
        {
            await MessageAsync(result.Status + ": " + result.Message);
        }
    }

    private async Task<List<TelegramMedia>> GetCachedMediaAsync(ImageResponse[] images)
    {
        var imagesKeys = images.Select(x => x.ContentKey);
        var cachedMedia = await _context.Medias
            .Where(x => imagesKeys.Contains(x.ContentKey))
            .ToListAsync();

        var cachedKeys = cachedMedia.Select(x => x.ContentKey);
        _uncachedImages = images.Where(x => !cachedKeys.Contains(x.ContentKey)).ToList();

        return cachedMedia;
    }

    private async Task SendMediaAsync()
    {
        var mediaList = _inputMedia.Select(
            x => new InputMediaPhoto(x)
        );

        var messages = await BotClient.SendMediaGroupAsync(
            ChatId,
            media: mediaList
        );

        await CacheMediaAsync(messages);
    }

    private async Task CacheMediaAsync(Message[] messages)
    {
        var fileIds = messages.Select(
            x => x.Photo!.Last().FileId
        ).ToList();

        var telegramMedia = new List<TelegramMedia>();
        for (var i = _uncachedImages.Count; i > 0; i--)
        {
            var fileId = fileIds.Last();
            telegramMedia.Add(new TelegramMedia
            {
                FileId = fileId,
                ContentKey = _uncachedImages[i - 1].ContentKey,
                MediaType = TelegramMediaType.Photo
            });
            fileIds.Remove(fileId);
        }

        if (telegramMedia.Any())
        {
            await _context.AddRangeAsync(telegramMedia);
            await _context.SaveChangesAsync();
        }
    }
}