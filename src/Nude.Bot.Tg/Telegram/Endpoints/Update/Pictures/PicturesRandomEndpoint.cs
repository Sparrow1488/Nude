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
    private List<ImageResponse> _notCachedPhotos = new List<ImageResponse>();

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
            var cachedMedia = await GetCachedMediaAsync(result);
            var inputMedia = new List<InputMedia>();

            cachedMedia.ForEach(x => {
                var media = new InputMedia(x.FileId);
                inputMedia.Add(media);
            });

            using var client = new HttpClient();
            var fileStreamsList = new List<Stream>();
            
            foreach (var image in _notCachedPhotos)
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
    
    private async Task<TelegramMedia> CacheMediaAsync(string fileId, ImageResponse image)
    {
        var mediaEntity = new TelegramMedia
        {
            FileId = fileId,
            ContentKey = image.ContentKey,
            MediaType = TelegramMediaType.Photo
        };
        
        await _context.AddAsync(mediaEntity);
        await _context.SaveChangesAsync();
        
        return mediaEntity;
    }

    private async Task<List<TelegramMedia>> GetCachedMediaAsync(ApiResult<ImageResponse[]> result)
    {
        var media = new List<TelegramMedia>();
        
        foreach (var image in result.ResultValue)
        {
            var dbImage = await _context.Medias
                .FirstOrDefaultAsync(x => x.ContentKey == image.ContentKey);

            if (dbImage != null)
            {
                media.Add(dbImage);
            }
            else
            {
                _notCachedPhotos.Add(image);
            }
        }
        
        return media;
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

        var idList = (messages.ToList().Select(x => 
            x.Photo!.Last().FileId))
            .ToList();

        for (int i = _notCachedPhotos.Count; i > 0; i--)
        {
            string fileId = idList.Last();
            await CacheMediaAsync(fileId, _notCachedPhotos[i - 1]);
            idList.Remove(fileId);
        }
    }
}