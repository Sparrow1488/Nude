using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nude.API.Infrastructure.Constants;
using Nude.Tg.Bot.Clients.Nude;
using Nude.Tg.Bot.Services.Convert;
using Nude.Tg.Bot.Services.Manga;
using Nude.Tg.Bot.Services.Messages;
using Nude.Tg.Bot.Telegram.Endpoints.Base;

namespace Nude.Tg.Bot.Telegram.Endpoints.Update;

public class MangaEndpoint : TelegramUpdateEndpoint
{
    private readonly ILogger<MangaEndpoint> _logger;
    private readonly INudeClient _nudeClient;
    private readonly IConfiguration _configuration;
    private readonly IMessagesStore _messages;
    private readonly ITelegraphMangaService _mangaService;
    private readonly IConvertTicketsService _ticketsService;

    public MangaEndpoint(
        INudeClient nudeClient,
        IConfiguration configuration,
        IMessagesStore messages,
        ITelegraphMangaService mangaService,
        IConvertTicketsService ticketsService,
        ILogger<MangaEndpoint> logger)
    {
        _logger = logger;
        _nudeClient = nudeClient;
        _configuration = configuration;
        _messages = messages;
        _mangaService = mangaService;
        _ticketsService = ticketsService;
    }
    
    public override async Task HandleAsync()
    {
        var mangaResponse = await _nudeClient.GetMangaByUrlAsync(MessageText);
        
        if (mangaResponse is null)
        {
            var callbackUrl = _configuration["Http:BaseUrl"] + "/callback";
            
            var response = await _nudeClient.CreateParsingTicketAsync(MessageText, callbackUrl);
            await _ticketsService.CreateAsync(response.Id, ChatId);

            await MessageAsync("Обработка не займет много времени. Я напишу Вам, когда все будет готово:)\n");
            return;
        }
        
        var manga = mangaResponse.Value;
    
        var tghExists = await _mangaService.GetByExternalIdAsync(manga.ExternalId);
        if (tghExists is not null)
        {
            await MessageAsync(await _messages.GetTghMessageAsync(tghExists));
            return;
        }

        await MessageAsync("Запрос обрабатывается! Спасибо за ожидание:)");
    }

    public override bool CanHandle() => AvailableSources.IsAvailable(Update.Message?.Text ?? "");
}