using Nude.API.Contracts.Images.Responses;
using Nude.API.Models.Messages;
using Nude.API.Models.Messages.Details;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Models.Api;
using Nude.Bot.Tg.Services.Messages.Service;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Utils;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Nude.Data.Infrastructure.Contexts;
using Telegram.Bot;
using Telegram.Bot.Types;
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
        var editMessageId = 0;
        var currentPhotoProcessing = 1;
        UserMessage? userMessage = null;

        var mediaGroupId = Update.Message!.MediaGroupId;
        if (mediaGroupId != null)
        {
            var previous = await _service.FindByMediaGroupIdAsync(mediaGroupId);
            editMessageId = previous?.MessageId ?? 0;
            
            if (previous == null)
            {
                editMessageId = (await SendStartMessageAsync()).MessageId;
            }
            
            userMessage = await CreateOrUpdateMessageAsync(
                editMessageId,
                mediaGroupId!,
                currentPhotoProcessing
            );
            currentPhotoProcessing = (userMessage.Details as MediaGroupDetails)!.CurrentMedia;
        }
        else
        {
            editMessageId = (await SendStartMessageAsync()).MessageId;
        }
        
        var result = await UploadPhotoAsync();
        
        if (result.IsSuccess)
        {
            var messageText = $"Содержимое `#{currentPhotoProcessing}` успешно загружено";
            await BotUtils.EditMessageAsync(
                BotClient,
                ChatId,
                editMessageId,
                new MessageItem(messageText, ParseMode.MarkdownV2)
            );
        }
        else
        {
            await MessageAsync(result.Status + ": " + result.Message);
        }
    }

    private Task<Message> SendStartMessageAsync()
    {
        return MessageAsync("Загрузка пошла");
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
    
    private async Task<UserMessage> CreateOrUpdateMessageAsync(
        int messageId,
        string mediaGroupId, 
        int current)
    {
        var message = await _service.FindByMediaGroupIdAsync(mediaGroupId);
    
        if (message == null)
        {
            var mediaDetails = new MediaGroupDetails
            {
                MediaGroupId = mediaGroupId,
                CurrentMedia = current
            };
            message = await _service.CreateAsync(ChatId, messageId, mediaDetails, UserSession.User);
        }
        else
        {
            var mediaDetails = message.Details as MediaGroupDetails;
            mediaDetails!.CurrentMedia += 1;
    
            message = await _service.UpdateAsync(message.Id, mediaDetails);
        }
    
        return message;
    }
}