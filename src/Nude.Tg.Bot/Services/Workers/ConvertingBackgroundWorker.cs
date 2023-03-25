using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Parsing.Responses;
using Nude.API.Data.Contexts;
using Nude.API.Infrastructure.Services.Background;
using Nude.Models.Mangas;
using Nude.Models.Tickets.Converting;
using Nude.Models.Tickets.Parsing;
using Nude.Tg.Bot.Clients.Nude;
using Nude.Tg.Bot.Clients.Telegraph;
using Nude.Tg.Bot.Services.Messages;
using Nude.Tg.Bot.Services.Messages.Store;
using Nude.Tg.Bot.Services.Messages.Telegram;
using Nude.Tg.Bot.Services.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Nude.Tg.Bot.Services.Workers;

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
            var message = await _messages.GetTghMessageAsync(tghManga);
            await BotUtils.MessageAsync(BotClient, chat, message);
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
        var dbMessage = await _tgMessagesService.GetByTicketIdAsync(convertTicketId);
        
        var totalImages = manga.Images.Count;

        for (var i = 0; i < manga.Images.Count; i++)
        {
            var tghImage = await _telegraph.UploadFileAsync(manga.Images[i]);
            convertedImages.Add(tghImage);

            if (i % 5 == 0 || i == manga.Images.Count - 1)
            {
                _logger.LogInformation(
                    "({current}/{total}) Uploading images to telegraph",
                    i,
                    totalImages);
            }
            
            await BotClient.EditMessageTextAsync(
                new ChatId(dbMessage!.ChatId),
                dbMessage!.MessageId,
                $"Происходит жоски загрузка картинок на сервер: ({i+1}/{totalImages})",
                ParseMode.Html
            );
        }

        return convertedImages;
    }
}
