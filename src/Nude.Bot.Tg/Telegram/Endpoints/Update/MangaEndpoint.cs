using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nude.API.Contracts.Formats.Responses;
using Nude.API.Contracts.Parsing.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Infrastructure.Constants;
using Nude.API.Models.Formats;
using Nude.API.Models.Messages;
using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.States;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Services.Convert;
using Nude.Bot.Tg.Services.Manga;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Messages.Telegram;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Nude.Models.Messages.Telegram;
using Nude.Models.Tickets.Converting;
using Nude.Models.Tickets.Parsing;
using Nude.Data.Infrastructure.Contexts;
using Telegram.Bot.Types.Enums;

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
    private readonly FixetBotDbContext _context;

    public MangaEndpoint(
        INudeClient nudeClient,
        IConfiguration configuration,
        IMessagesStore messages,
        ITelegraphMangaService mangaService,
        IConvertTicketsService ticketsService,
        ITelegramMessagesService tgMessagesService,
        FixetBotDbContext context,
        ILogger<MangaEndpoint> logger)
    {
        _logger = logger;
        _nudeClient = nudeClient;
        _configuration = configuration;
        _messages = messages;
        _mangaService = mangaService;
        _ticketsService = ticketsService;
        _context = context;
        _tgMessagesService = tgMessagesService;
    }
    
    public override async Task HandleAsync()
    {
        var mangaResponse = await _nudeClient.GetMangaByUrlAsync(MessageText, FormatType.Telegraph);

        if (mangaResponse is not null)
        {
            var manga = mangaResponse.Value;
            var tghFormat = (TelegraphContentResponse) manga.Formats.First(x => x is TelegraphContentResponse);
            await MessageAsync(await _messages.GetTghMessageAsync(tghFormat.Url));
            return;
        }
        else
        {
            var callbackUrl = _configuration["Http:BaseUrl"] + "/callback";
            var request = new ContentTicketRequest
            {
                SourceUrl = MessageText,
                CallbackUrl = callbackUrl
            };
            var response = await _nudeClient.CreateContentTicket(request);
            await MessageAsync(new MessageItem("Нужно немного подождать...", ParseMode.MarkdownV2));
            _context.Add(new UserMessages
            {
                ChatId = Update.Message.Chat.Id,
                MessageId = Update.Message.MessageId, 
                UserId = Update.Message.From.Id
            });
        }
    }

    public override bool CanHandle() => AvailableSources.IsAvailable(Update.Message?.Text ?? "");
}














