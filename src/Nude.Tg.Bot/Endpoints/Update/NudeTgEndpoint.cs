using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Parsing.Responses;
using Nude.API.Data.Contexts;
using Nude.Models.Mangas;
using Nude.Models.Tickets.Converting;
using Nude.Tg.Bot.Clients.Nude;
using Nude.Tg.Bot.Clients.Telegraph;
using Nude.Tg.Bot.Endpoints.Base;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nude.Tg.Bot.Endpoints.Update;

public class NudeTgEndpoint : TelegramUpdateEndpoint
{
    private readonly ITelegraphClient _telegraph;
    private readonly BotDbContext _context;
    private readonly ILogger<NudeTgEndpoint> _logger;
    private readonly INudeClient _nudeClient;
    private readonly IConfiguration _configuration;

    public NudeTgEndpoint(
        ITelegraphClient telegraph, 
        BotDbContext context,
        INudeClient nudeClient,
        IConfiguration configuration,
        ILogger<NudeTgEndpoint> logger)
    {
        _telegraph = telegraph;
        _context = context;
        _logger = logger;
        _nudeClient = nudeClient;
        _configuration = configuration;
    }
    
    public override async Task HandleAsync()
    {
        try
        {
            var mangaResponse = await _nudeClient.GetMangaByUrlAsync(MessageText);
            
            if (mangaResponse is null)
            {
                await OnMangaNotExists();
                return;
            }

            var manga = mangaResponse.Value;
            if (manga.Images.Count > 40)
            {
                await OnMangaTooLongAsync();
                return;
            }

            await OnSendMangaResponseAsync(manga);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Что-то пошло не так");
            await BotClient.SendTextMessageAsync(ChatId, "😓 Упс! Что-то пошло не так");
        }
    }

    private async Task OnMangaNotExists()
    {
        var callbackUrl = _configuration["Http:BaseUrl"] + "/callback";
        await BotClient.SendTextMessageAsync(ChatId, NoContentMessage());
        var parsingResponse = await _nudeClient.CreateParsingTicketAsync(
            MessageText, 
            callbackUrl);

        await _context.AddAsync(new ConvertingTicket
        {
            ChatId = ChatId,
            ParsingTicketId = parsingResponse.Id,
            Status = ConvertingStatus.Frozen
        });
        await _context.SaveChangesAsync();
        
        await BotClient.SendTextMessageAsync(ChatId, GetParsingMessage(parsingResponse));
    }

    private async Task OnMangaTooLongAsync()
    {
        await BotClient.SendTextMessageAsync(ChatId, "Слишком большая манга! С-Cервер может не выдержать...");
    }

    private async Task OnSendMangaResponseAsync(MangaResponse manga)
    {
        var tghExists = await _context.TghMangas
            .FirstOrDefaultAsync(x => x.ExternalId == manga.ExternalId);

        if (tghExists is not null)
        {
            await BotClient.SendTextMessageAsync(ChatId, tghExists.TghUrl);
            return;
        }
        
        await BotClient.SendTextMessageAsync(ChatId, "Манга найдена, но ее обработка займет какое-то время\nСпасибо за ожидание!");
    }

    public override bool CanHandle()
    {
        return Update.Message?.Text?.Contains("nude-moon.org") ?? false;
    }

    private static string NoContentMessage()
        => "🫣 Ничего нет!\n" +
           "Но не переживайте, мы уведомим вас, как только манга появится :)";

    private static string GetParsingMessage(ParsingResponse response)
        => $"Id:{response.Id}\nStatus: {response.Status}\nCode: {response.Result.StatusCode}\nMessage: {response.Result.Message}";
}