using Nude.API.Contracts.Images.Responses;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Models.Api;
using Nude.Bot.Tg.Services.Messages.Service;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Utils;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Nude.Data.Infrastructure.Contexts;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Pictures;

public class PictureUploadEndpoint : TelegramUpdateEndpoint
{
    private readonly INudeClient _client;
    private readonly IMessageService _service;

    public PictureUploadEndpoint(
        INudeClient client,
        IMessageService service)
    {
        _client = client;
        _service = service;
    }
    
    public override bool CanHandle() => Update.Message?.Photo != null;
    
    public override async Task HandleAsync()
    {
        var currentPhotoProcessing = 1;
        
        var message = await MessageAsync("Загрузка пошла");
        var editMessageId = message.MessageId;

        var result = await UploadPhotoAsync();
        
        if (result.IsSuccess)
        {
            var messageText = $"#{currentPhotoProcessing} успешно загружено";
            await BotUtils.EditMessageAsync(
                BotClient,
                ChatId,
                editMessageId,
                new MessageItem(messageText, ParseMode.Html)
            );
        }
        else
        {
            await MessageAsync(result.Status + ": " + result.Message);
        }
    }

    private async Task<ApiResult<ImageResponse>> UploadPhotoAsync()
    {
        var uploadSize = Update.Message!.Photo!.Last();
        
        await using var memory = new MemoryStream();
        var file = await BotClient.GetFileAsync(uploadSize.FileId);
        if (file.FilePath == null)
        {
            throw new Exception("Telegram FilePath is null");
        }
        
        await BotClient.DownloadFileAsync(file.FilePath, memory);

        memory.Seek(0, SeekOrigin.Begin);
        var bytes = memory.ToArray();

        var authorizeClient = _client.AuthorizeClient(UserSession);
        return await authorizeClient.CreateImageAsync(
            bytes,
            file.FilePath
        );
    }
    
    // private async Task<UserMessage> CreateOrUpdateMessageAsync(
    //     int messageId,
    //     string mediaGroupId, 
    //     int current, 
    //     int total)
    // {
    //     var message = await _service.FindByMediaGroupIdAsync(mediaGroupId);
    //
    //     if (message == null)
    //     {
    //         var mediaDetails = new MediaGroupDetails
    //         {
    //             MediaGroupId = mediaGroupId,
    //             CurrentMedia = current,
    //             TotalMedia = total
    //         };
    //         message = await _service.CreateAsync(ChatId, messageId, mediaDetails, UserSession.User);
    //     }
    //     else
    //     {
    //         var mediaDetails = message.Details as MediaGroupDetails;
    //         mediaDetails!.CurrentMedia += 1;
    //         mediaDetails.TotalMedia = total;
    //
    //         message = await _service.UpdateAsync(message.Id, mediaDetails);
    //     }
    //
    //     return message;
    // }
}