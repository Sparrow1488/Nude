using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Parsing.Responses;
using Nude.Data.Infrastructure.Contexts;
using Nude.API.Infrastructure.Services.Background;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Clients.Telegraph;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Messages.Telegram;
using Nude.Bot.Tg.Services.Utils;
using Nude.Models.Mangas;
using Nude.Models.Messages.Telegram;
using Nude.Models.Tickets.Converting;
using Nude.Models.Tickets.Parsing;
using Telegram.Bot;

namespace Nude.Bot.Tg.Services.Workers;

public sealed class ConvertingBackgroundWorker : IBackgroundWorker
{
    private readonly IMessagesStore _messages;
    private readonly ITelegraphClient _telegraph;
    private readonly INudeClient _nudeClient;
    private readonly ITelegramMessagesService _tgMessagesService;
    private readonly ILogger<ConvertingBackgroundWorker> _logger;
    private readonly BotDbContext _context;

    public ConvertingBackgroundWorker(
        ITelegramBotClient botClient,
        BotDbContext context,
        IMessagesStore messages,
        ITelegraphClient telegraph,
        INudeClient nudeClient,
        ITelegramMessagesService tgMessagesService,
        ILogger<ConvertingBackgroundWorker> logger)
    {
        _context = context;
        _messages = messages;
        _telegraph = telegraph;
        _nudeClient = nudeClient;
        _tgMessagesService = tgMessagesService;
        _logger = logger;
        BotClient = botClient;
    }
    
    private ITelegramBotClient BotClient { get; }
    
    public async Task ExecuteAsync(BackgroundServiceContext ctx, CancellationToken ctk)
    {
        try
        {
            var ticket = await _context.ConvertingTickets
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(x => 
                        x.Status == ConvertingStatus.ConvertWaiting, cancellationToken: ctk);

            if (ticket is not null)
            {
                await OnProcessTicketAsync(ticket);
            }
            else
            {
                _logger.LogInformation("No converting tickets");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), ctk);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Oops.. Something went wrong");
        }
    }

    private async Task OnProcessTicketAsync(ConvertingTicket ticket)
    {
        var (parsingTicket, status) = await GetParsingTicketStatusAsync(ticket.ParsingId);

        if (parsingTicket is null || status != ParsingStatus.Success)
        {
            _logger.LogError("Failed to process non-existing or failed ticket");

            ticket.Status = ConvertingStatus.Failed;
            await _context.SaveChangesAsync();
            return;
        }

        var mangaId = int.Parse(parsingTicket.Value.Result.EntityId!);
        var manga = (await _nudeClient.GetMangaByIdAsync(mangaId))!.Value;

        var tghManga = await CreateTghMangaAsync(manga, ticket.Id);

        var similarTickets = await _context.ConvertingTickets
            .Where(x => x.ParsingId == ticket.ParsingId)
            .ToListAsync();
        
        similarTickets.ForEach(x => x.Status = ConvertingStatus.Success);
        
        await _context.AddAsync(tghManga);
        await _context.SaveChangesAsync();

        var chats = similarTickets
            .DistinctBy(x => x.ChatId)
            .Select(x => x.ChatId);

        foreach (var chat in chats)
        {
            var dbMessage = await _tgMessagesService.GetByTicketIdAsync(ticket.Id);
            var message = await _messages.GetTghMessageAsync(tghManga);
            await BotUtils.EditMessageAsync(
                BotClient, 
                chat,
                dbMessage!.MessageId,
                message
            );
        }
    }

    private async Task<(ParsingResponse?, ParsingStatus)> GetParsingTicketStatusAsync(int parsingTicketId)
    {
        var parsingResponse = await _nudeClient.GetParsingTicketAsync(parsingTicketId);

        if (parsingResponse is null)
        {
            _logger.LogError("Parsing Ticket not exists yet");
            return (null, ParsingStatus.Failed);
        }

        var parsingTicket = parsingResponse.Value;
        
        _logger.LogInformation(
            "[{id}] Parsing Ticket status:{status}, code:{code}, message:{message}",
            parsingTicket.Id,
            parsingTicket.Status,
            parsingTicket.Result.StatusCode,
            parsingTicket.Result.Message);

        return (parsingTicket, parsingTicket.Status);
    }

    private async Task<TghManga> CreateTghMangaAsync(MangaResponse manga, int convertTicketId)
    {
        _logger.LogInformation("Converting manga images ({total})", manga.Images.Count);
        
        manga.Images = await UploadToTelegraphImagesAsync(manga, convertTicketId);
        
        var tghUrl = await _telegraph.CreatePageAsync(manga);
        return new TghManga
        {
            ExternalId = manga.ExternalId,
            TghUrl = tghUrl
        };
    }

    private async Task<List<string>> UploadToTelegraphImagesAsync(MangaResponse manga, int convertTicketId)
    {
        var convertedImages = new List<string>();
        var dbMessages = await _tgMessagesService
            .GetSimilarByTicketIdAsync(convertTicketId);
        
        var totalImages = manga.Images.Count;

        for (var i = 0; i < manga.Images.Count; i++)
        {
            var tghImage = await _telegraph.UploadFileAsync(manga.Images[i]);
            convertedImages.Add(tghImage);

            var currentImage = i + 1;
            if (currentImage % 5 == 0 || i == manga.Images.Count - 1)
            {
                _logger.LogInformation(
                    "({current}/{total}) Uploading images to telegraph",
                    currentImage,
                    totalImages);
            }

            await UpdateMessagesAsync(dbMessages, currentImage, totalImages);
        }

        return convertedImages;
    }

    private async Task UpdateMessagesAsync(
        IEnumerable<TelegramConvertingMessage> messages,
        int currentImage,
        int totalImages)
    {
        foreach (var message in messages)
        {
            var tgMessage = await BotUtils.EditMessageAsync(
                BotClient,
                message.ChatId,
                message.MessageId,
                await _messages.GetImagesUploadMessageAsync(currentImage, totalImages)
            );

            if (tgMessage == null)
            {
                _logger.LogWarning("Editing message respond null message object. Remove it from DB");
                await _tgMessagesService.DeleteMessageAsync(message);
            }
        }
    }
}
