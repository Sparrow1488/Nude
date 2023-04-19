using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Messages;
using Nude.API.Models.Messages.Details;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Users;
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
    private readonly BotDbContext _context;

    public PictureUploadEndpoint(
        INudeClient client,
        BotDbContext context)
    {
        _client = client;
        _context = context;
    }
    
    public override bool CanHandle() => Update.Message?.Photo != null;
    
    public override async Task HandleAsync()
    {
        var sizes = Update.Message!.Photo!;
        var uploadSize = sizes.Last();

        var messageId = 0;
        var previous = await FindMessageAsync(Update.Message.MediaGroupId!);
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
                (int) userMessage.MessageId,
                new MessageItem(message, ParseMode.Html)
            );
        }
        else
        {
            await MessageAsync(result.Status + ": " + result.Message);
        }
    }

    private async Task<UserMessage?> FindMessageAsync(string mediaGroupId)
    {
        var chatMessages = await _context.Messages
            .Where(x => x.Details is MediaGroupDetails && x.ChatId == ChatId)
            .ToListAsync();
        return chatMessages.FirstOrDefault(
            x => x.Details is MediaGroupDetails details
                 && details.MediaGroupId == mediaGroupId
        );
    }

    private async Task<UserMessage> CreateOrUpdateMessageAsync(
        int messageId,
        string? mediaGroupId, 
        int current, 
        int total)
    {
        var exists = await _context.Messages
            .Include(x => x.Details)
            .Where(x => x.ChatId == ChatId && x.Details is MediaGroupDetails)
            .ToListAsync();

        var currentMessage = exists.FirstOrDefault(
            x => x.Details is MediaGroupDetails det && det.MediaGroupId == mediaGroupId
        );

        if (currentMessage == null)
        {
            var message = new UserMessage
            {
                ChatId = ChatId,
                MessageId = messageId,
                Details = new MediaGroupDetails
                {
                    MediaGroupId = mediaGroupId ?? "",
                    TotalMedia = total,
                    CurrentMedia = current
                },
                OwnerId = UserSession.User.Id
            };

            await _context.AddAsync(message);
            await _context.SaveChangesAsync();

            return message;
        }
        else
        {
            var details = currentMessage.Details as MediaGroupDetails;
            details!.CurrentMedia++;
            details.TotalMedia = total;

            await _context.SaveChangesAsync();

            return currentMessage;
        }
    }
}