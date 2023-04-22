using System.Net.Sockets;
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

    public PicturesRandomEndpoint(
        INudeClient client,
        BotDbContext context) 
    : base(NavigationDefaults.RandomPicture)
    {
        _client = client;
        _context = context;
    }
    private async Task<List<TelegramMedia>> GetMediaFromDb(ApiResult<ImageResponse[]> result)
    {
        var media = new List<TelegramMedia>();
        foreach (var image in result.Result!)
        {
            var dbImage = await _context.Medias.FirstOrDefaultAsync(x => x.ContentKey == image.ContentKey);
            if (dbImage != null) 
            {
                media.Add(dbImage);
            }
            else
            {
                var mediEntity = new TelegramMedia
                {
                    ContentKey = image.ContentKey,
                    MediaType = TelegramMediaType.Photo,
                };
            }
        }
        return media;
    }
    public override async Task HandleAsync()
    {
        var result = await _client.GetRandomImagesAsync();
        if (result.IsSuccess)
        {
            var media = await GetMediaFromDb(result);
            using var client = new HttpClient();
            var fileStreamsList = new List<Stream>();
            foreach (var image in result.ResultValue)
            {
                var stream = await client.GetStreamAsync(image.Url);
                fileStreamsList.Add(stream);
            }

            var mediaList = fileStreamsList.Select(
                x => new InputMediaPhoto(new InputMedia(x, x.GetHashCode() + "-nude-bot-image.png")
            ));
            await BotClient.SendMediaGroupAsync(
                ChatId,
                media: mediaList
            );

            fileStreamsList.ForEach(x => x.Close());

            // await CreateMediaAsync(result.ResultValue, messages);
        }
        else
        {
            await MessageAsync(result.Status + ": " + result.Message);
        }
    }

    // private async Task<InputMediaPhoto> CreateRequestMediaAsync(ImageResponse[] images)
    // {
    //     var contentKeys = images.Select(x => x.ContentKey);
    //     var existsMediaList = await _context.Medias
    //         .Where(x => contentKeys.Contains(x.ContentKey))
    //         .ToListAsync();
    //
    //     var existsContentKeys = existsMediaList.Select(x => x.ContentKey);
    //     var uploadMediaList = images.Where(
    //         x => !existsContentKeys.Contains(x.ContentKey));
    //
    //     var mediaList = new List<InputMediaPhoto>();
    //     mediaList.AddRange(existsMediaList.Select(
    //         x => new InputMediaPhoto(new InputMedia(x.FileId))
    //     ));
    //     mediaList.AddRange(uploadMediaList.Select(
    //         x => new InputMediaPhoto()));
    // }

    // private async Task CreateMediaAsync(
    //     ImageResponse[] images,
    //     Message[] mediaMessages)
    // {
    //     var mediaList = new List<TelegramMedia>();
    //     for (var i = 0; i < images.Length; i++)
    //     {
    //         var image = images[i];
    //         var media = mediaMessages[i];
    //         
    //         mediaList.Add(new TelegramMedia
    //         {
    //             ContentKey = image.ContentKey,
    //             FileId = media.Photo!.Last().FileId,
    //             MediaType = TelegramMediaType.Photo
    //         });
    //     }
    //
    //     await _context.AddRangeAsync(mediaList);
    //     await _context.SaveChangesAsync();
    // }
}