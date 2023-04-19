using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Messages;
using Nude.API.Models.Messages.Details;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
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
    private readonly BotDbContext _context;
    private readonly IMessageService _service;

    public PictureUploadEndpoint(
        INudeClient client,
        BotDbContext context,
        IMessageService service)
    {
        _client = client;
        _context = context;
        _service = service;
    }
    
    public override bool CanHandle() => Update.Message?.Photo != null;
    
    public override async Task HandleAsync()
    {
        var messageId = 0;
        var previous = await _service.FindAsync(
            ChatId, 
            UserSession.User, 
            Update.Message!.MediaGroupId
        );
        
        if (previous == null)
        {
            var startMessage = await MessageAsync("Загрузка пошла");
            messageId = startMessage.MessageId;
        }
        
        var userMessage = await CreateOrUpdateMessageAsync(
            messageId, 
            Update.Message.MediaGroupId, 
            1, 
            -1
        );
        
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
        var result = await authorizeClient.CreateImageAsync(
            bytes,
            file.FilePath
        );

        if (result.IsSuccess)
        {
            var currentMedia = (userMessage.Details as MediaGroupDetails)!.CurrentMedia;

            var message = $"#{currentMedia} успешно загружено";
            await BotUtils.EditMessageAsync(
                BotClient,
                ChatId,
                userMessage.MessageId,
                new MessageItem(message, ParseMode.Html)
            );
        }
        else
        {
            await MessageAsync(result.Status + ": " + result.Message);
        }
    }

    private async Task<UserMessage> CreateOrUpdateMessageAsync(
        int messageId,
        string? mediaGroupId, 
        int current, 
        int total)
    {
        var message = await _service.FindAsync(ChatId, UserSession.User, mediaGroupId);

        if (message == null)
        {
            var mediaDetails = new MediaGroupDetails
            {
                MediaGroupId = mediaGroupId,
                CurrentMedia = current,
                TotalMedia = total
            };
            message = await _service.CreateAsync(ChatId, messageId, mediaDetails, UserSession.User);
        }
        else
        {
            var mediaDetails = message.Details as MediaGroupDetails;
            mediaDetails!.CurrentMedia += 1;
            mediaDetails.TotalMedia = total;

            message = await _service.UpdateAsync(message.Id, mediaDetails);
        }

        return message;
    }
}