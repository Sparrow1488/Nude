using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nude.API.Data.Contexts;
using Nude.API.Infrastructure.Services.Background;
using Nude.Models.Mangas;
using Nude.Models.Tickets.Converting;
using Nude.Models.Tickets.Parsing;
using Nude.Tg.Bot.Clients.Nude;
using Nude.Tg.Bot.Clients.Telegraph;
using Nude.Tg.Bot.Services.Messages;
using Nude.Tg.Bot.Services.Utils;
using Telegram.Bot;

namespace Nude.Tg.Bot.Services.Background;

public sealed class ConvertBgService : BgService
{
    private readonly IDbContextFactory<BotDbContext> _contextFactory;
    private readonly IMessagesStore _messages;
    private readonly ITelegraphClient _telegraph;
    private readonly INudeClient _nudeClient;
    private readonly ILogger<ConvertBgService> _logger;
    
    private BotDbContext _context = null!;

    public ConvertBgService(
        ITelegramBotClient botClient,
        IDbContextFactory<BotDbContext> contextFactory,
        IMessagesStore messages,
        ITelegraphClient telegraph,
        INudeClient nudeClient,
        ILogger<ConvertBgService> logger)
    {
        _contextFactory = contextFactory;
        _messages = messages;
        _telegraph = telegraph;
        _nudeClient = nudeClient;
        _logger = logger;
        BotClient = botClient;
    }
    
    private ITelegramBotClient BotClient { get; }
    
    protected override async Task ExecuteAsync()
    {
        while (true)
        {
            try
            {
                _context = await _contextFactory.CreateDbContextAsync();

                var ticket = await _context.ConvertingTickets
                    .FirstOrDefaultAsync(x => x.Status == ConvertingStatus.WaitToProcess);

                if (ticket is not null)
                {
                    await OnProcessTicketAsync(ticket);
                }
                else
                {
                    _logger.LogInformation("No converting tickets");
                }

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Че то не так");
            }
            finally
            {
                await _context.DisposeAsync();
            }
        }
    }

    private async Task OnProcessTicketAsync(ConvertingTicket ticket)
    {
        var parsingResponse = await _nudeClient.GetParsingTicketAsync(ticket.ParsingId);

        if (parsingResponse is null)
        {
            _logger.LogError("Parsing Ticket not exists yet");
            return;
        }

        var parsingTicket = parsingResponse.Value;
        var status = Enum.Parse<ParsingStatus>(parsingTicket.Status, true);
        
        _logger.LogInformation(
            "[{id}] Parsing Ticket status:{status}, code:{code}, message:{message}",
            parsingTicket.Id,
            parsingTicket.Status,
            parsingTicket.Result.StatusCode,
            parsingTicket.Result.Message);

        if (status != ParsingStatus.Success) return;

        var mangaId = int.Parse(parsingTicket.Result.EntityId!);
        var mangaResponse = await _nudeClient.GetMangaByIdAsync(mangaId);

        var manga = mangaResponse!.Value;

        var convertedImages = new List<string>();
        foreach (var image in manga.Images)
        {
            var tghImage = await _telegraph.UploadFileAsync(image);
            convertedImages.Add(tghImage);
        }
        
        manga.Images = convertedImages;
        
        var tghUrl = await _telegraph.CreatePageAsync(manga);
        
        ticket.Status = ConvertingStatus.Success;

        var tghManga = new TghManga
        {
            ExternalId = manga.ExternalId,
            TghUrl = tghUrl
        };
        
        await _context.AddAsync(tghManga);
        await _context.SaveChangesAsync();

        var message = await _messages.GetTghMessageAsync(tghManga);
        await BotUtils.MessageAsync(BotClient, ticket.ChatId, message);
    }
}
