using Microsoft.EntityFrameworkCore;
using Nude.API.Contracts.Images.Responses;
using Nude.API.Models.Enums;
using Nude.API.Models.Media;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Constants;
using Nude.Bot.Tg.Models.Api;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Nude.Data.Infrastructure.Contexts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Pictures;

public class PicturesRandomEndpoint : TelegramUpdateCommandEndpoint
{
    private readonly INudeClient _client;
    private readonly BotDbContext _context;
    private readonly IMessagesStore _messages;

    public PicturesRandomEndpoint(
        INudeClient client,
        BotDbContext context,
        IMessagesStore messages) 
    : base(NavigationDefaults.RandomPicture)
    {
        _client = client;
        _context = context;
        _messages = messages;
    }

    public override async Task HandleAsync()
    {
        var result = await _client.GetRandomImagesAsync();
        if (result.IsSuccess)
        {
            var mediaList = await GetCachedMediaAsync(result);
            
            using var client = new HttpClient();
            var fileStreamsList = new List<Stream>();
            
            foreach (var image in mediaList)
            {
                var stream = await client.GetStreamAsync(image.FileId);
                fileStreamsList.Add(stream);
            }

            await SendMedia(fileStreamsList);
        }
        else
        {
            var badMessage = await _messages.GetErrorResponseMessageAsync(result.ErrorValue);
            await MessageAsync(badMessage);
        }
    }
    
    private async Task<TelegramMedia> CacheMediaAsync(ImageResponse image)
    {
        var mediaEntity = new TelegramMedia
        {
            FileId = image.Url,
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

            var mediaEntity = dbImage ?? await CacheMediaAsync(image);
            media.Add(mediaEntity);
        }
        
        return media;
    }

    private async Task SendMedia(List<Stream> fileStreamsList)
    {
        var mediaList = fileStreamsList.Select(
            x => new InputMediaPhoto(new InputMedia(x, x.GetHashCode() + "-nude-bot-image.png")
        ));

        var messages = await BotClient.SendMediaGroupAsync(
            ChatId,
            media: mediaList
        );

        fileStreamsList.ForEach(x => x.Close());
    }
}