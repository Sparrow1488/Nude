using Microsoft.EntityFrameworkCore;
using Nude.API.Contracts.Images.Responses;
using Nude.API.Models.Enums;
using Nude.API.Models.Media;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Constants;
using Nude.Bot.Tg.Models.Api;
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
    : base(NavigationDefaults.RandomPicture)
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
            var inputMedia = new List<InputMedia>();

            cachedMedia.ToList().ForEach(x => {
                var media = new InputMedia(x.FileId);
                inputMedia.Add(media);
            });

            using var client = new HttpClient();
            var fileStreamsList = new List<Stream>();
            
            foreach (var image in _uncachedImages)
            {
                var stream = await client.GetStreamAsync(image.Url);
                fileStreamsList.Add(stream);
                inputMedia.Add(
                    new InputMedia(stream, image.GetHashCode() + "-nude-bot-image.png")        
                );
            }

            await SendMediaAsync(inputMedia);
            fileStreamsList.ForEach(x => x.Close());
        }
        else
        {
            await MessageAsync(result.Status + ": " + result.Message);
        }
    }
    
    private async Task CacheMediaAsync(string fileId, ImageResponse image)
    {
        var mediaEntity = new TelegramMedia
        {
            FileId = fileId,
            ContentKey = image.ContentKey,
            MediaType = TelegramMediaType.Photo
        };
        
        await _context.AddAsync(mediaEntity);
        await _context.SaveChangesAsync();
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

    private async Task SendMediaAsync(List<InputMedia> media)
    {
        var mediaList = media.Select(
            x => new InputMediaPhoto(x)
        );

        var messages = await BotClient.SendMediaGroupAsync(
            ChatId,
            media: mediaList
        );

        var fileIds = messages.Select(
            x => x.Photo!.Last().FileId
        ).ToList();

        for (var i = _uncachedImages.Count; i > 0; i--)
        {
            var fileId = fileIds.Last();
            await CacheMediaAsync(fileId, _uncachedImages[i - 1]);
            fileIds.Remove(fileId);
        }
    }
}