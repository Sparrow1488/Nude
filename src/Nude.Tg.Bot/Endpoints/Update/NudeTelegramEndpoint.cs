using Microsoft.Extensions.Logging;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Parsing.Responses;
using Nude.Tg.Bot.Clients.Nude;
using Nude.Tg.Bot.Clients.Telegraph;
using Telegram.Bot;

namespace Nude.Tg.Bot.Endpoints.Update;

public class NudeTelegramEndpoint : TelegramUpdateEndpoint
{
    private readonly ITelegraphClient _telegraph;
    private readonly ILogger<NudeTelegramEndpoint> _logger;
    private readonly INudeClient _nudeClient;

    public NudeTelegramEndpoint(ITelegraphClient telegraph, ILogger<NudeTelegramEndpoint> logger)
    {
        _telegraph = telegraph;
        _logger = logger;
        _nudeClient = new NudeClient();
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
        await BotClient.SendTextMessageAsync(ChatId, NoContentMessage());
        var parsingResponse = await _nudeClient.CreateParsingRequestAsync(MessageText, string.Empty);
        await BotClient.SendTextMessageAsync(ChatId, GetParsingMessage(parsingResponse));
    }

    private async Task OnMangaTooLongAsync()
    {
        await BotClient.SendTextMessageAsync(ChatId, "Слишком большая манга! С-Cервер может не выдержать...");
    }

    private async Task OnSendMangaResponseAsync(MangaResponse manga)
    {
        var convertedImages = new List<string>();
        foreach (var image in manga.Images)
        {
            var tghImage = await _telegraph.UploadFileAsync(image);
            convertedImages.Add(tghImage);
        }

        manga.Images = convertedImages;
        var tghUrl = await _telegraph.CreatePageAsync(manga);
        await BotClient.SendTextMessageAsync(ChatId, tghUrl);
    }

    public override bool CanHandle()
    {
        return Update.Message?.Text?.Contains("nude-moon.org") ?? false;
    }

    private static string NoContentMessage()
        => "🫣 Ничего нет!\n" +
           "Но не переживайте, мы уведомим вас, как только манга появится :)";

    private static string GetParsingMessage(ParsingResponse response)
        => $"Id:{response.UniqueId}\nStatus: {response.Status}\nMessage: {response.Message}";
}