using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nude.API.Infrastructure.Constants;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Services.Convert;
using Nude.Bot.Tg.Services.Manga;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Messages.Telegram;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Nude.Models.Messages.Telegram;
using Nude.Models.Tickets.Converting;
using Nude.Models.Tickets.Parsing;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class MangaEndpoint : TelegramUpdateEndpoint
{
    private readonly ILogger<MangaEndpoint> _logger;
    private readonly INudeClient _nudeClient;
    private readonly IConfiguration _configuration;
    private readonly IMessagesStore _messages;
    private readonly ITelegraphMangaService _mangaService;
    private readonly IConvertTicketsService _ticketsService;
    private readonly ITelegramMessagesService _tgMessagesService;

    public MangaEndpoint(
        INudeClient nudeClient,
        IConfiguration configuration,
        IMessagesStore messages,
        ITelegraphMangaService mangaService,
        IConvertTicketsService ticketsService,
        ITelegramMessagesService tgMessagesService,
        ILogger<MangaEndpoint> logger)
    {
        _logger = logger;
        _nudeClient = nudeClient;
        _configuration = configuration;
        _messages = messages;
        _mangaService = mangaService;
        _ticketsService = ticketsService;
        _tgMessagesService = tgMessagesService;
    }
    
    public override async Task HandleAsync()
    {
        var mangaResponse = await _nudeClient.GetMangaByUrlAsync(MessageText);

        if (mangaResponse is not null)
        {
            var manga = mangaResponse.Value;
    
            var tghExists = await _mangaService.GetByExternalIdAsync(manga.ExternalId);
            if (tghExists is not null)
            {
                await MessageAsync(await _messages.GetTghMessageAsync(tghExists));
                return;
            }
        }

        var callbackUrl = _configuration["Http:BaseUrl"] + "/callback";

        var response = await _nudeClient.CreateParsingTicketAsync(MessageText, callbackUrl);
        var convertStatus = response.Status == ParsingStatus.Success
            ? ConvertingStatus.ConvertWaiting
            : ConvertingStatus.ParseWaiting;
        
        var ticket = await _ticketsService.CreateAsync(response.Id, ChatId, convertStatus);

        var message = await MessageAsync("Обработка не займет много времени. Я напишу Вам, когда все будет готово:)\n");
        
        var convertingMessage = new TelegramConvertingMessage
        {
            MessageId = message.MessageId,
            Text = message.Text,
            ChatId = ChatId,
            ConvertTicketId = ticket.Id
        };
        await _tgMessagesService.CreateMessageAsync(convertingMessage);
    }

    public override bool CanHandle() => AvailableSources.IsAvailable(Update.Message?.Text ?? "");
}