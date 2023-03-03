using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Parsing.Responses;
using Nude.Tg.Bot.Clients.Nude;
using Nude.Tg.Bot.Services.Convert;
using Nude.Tg.Bot.Services.Manga;
using Nude.Tg.Bot.Telegram.Endpoints.Base;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Tg.Bot.Telegram.Endpoints.Update;

public class NudeTgEndpoint : TelegramUpdateEndpoint
{
    private readonly ILogger<NudeTgEndpoint> _logger;
    private readonly INudeClient _nudeClient;
    private readonly IConfiguration _configuration;
    private readonly ITelegraphMangaService _mangaService;
    private readonly IConvertTicketsService _ticketsService;

    public NudeTgEndpoint(
        INudeClient nudeClient,
        IConfiguration configuration,
        ITelegraphMangaService mangaService,
        IConvertTicketsService ticketsService,
        ILogger<NudeTgEndpoint> logger)
    {
        _logger = logger;
        _nudeClient = nudeClient;
        _configuration = configuration;
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
    
        var tghExists = await _mangaService.GetByExternalId(manga.ExternalId);
        if (tghExists is not null)
        {
            await MessageAsync(tghExists.TghUrl);
            return;
        }

        await MessageAsync("Запрос обрабатывается! Спасибо за ожидание:)");
    }
    
    public override bool CanHandle()
    {
        if (Uri.TryCreate(Update.Message?.Text, UriKind.RelativeOrAbsolute, out var url))
            return url.Host == "nude-moon.org";

        return false;
    }
}