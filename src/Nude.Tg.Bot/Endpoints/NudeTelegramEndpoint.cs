using Nude.API.Contracts.Parsing.Responses;
using Nude.Tg.Bot.Clients;
using Nude.Tg.Bot.Clients.Nude;
using Nude.Tg.Bot.Clients.Telegraph;
using Telegram.Bot;

namespace Nude.Tg.Bot.Endpoints;

public class NudeTelegramEndpoint : TelegramUpdateEndpoint
{
    private readonly ITelegraphClient _telegraph;

    public NudeTelegramEndpoint(ITelegraphClient telegraph)
    {
        _telegraph = telegraph;
    }
    
    public override async Task HandleAsync()
    {
        var client = new NudeClient();
        var messageText = Update.Message!.Text ?? "";
        var chatId = Update.Message!.Chat.Id;
        var mangaResponse = await client.GetMangaByUrlAsync(messageText);
            
        if (mangaResponse is null)
        {
            await BotClient.SendTextMessageAsync(chatId, NoContentMessage());
            var parsingResponse = await client.CreateParsingRequestAsync(messageText, string.Empty);
            await BotClient.SendTextMessageAsync(chatId, GetParsingMessage(parsingResponse));
            return;
        }

        var manga = mangaResponse.Value;
        if (manga.Images.Count > 40)
        {
            await BotClient.SendTextMessageAsync(chatId, "Слишком большая манга! С-сервер может не выдержать...");
            return;
        }
        
        var convertedImages = new List<string>();
        foreach (var image in manga.Images)
        {
            var tghImage = await _telegraph.UploadFileAsync(image);
            convertedImages.Add(tghImage);
        }

        manga.Images = convertedImages;
        var url = await _telegraph.CreatePageAsync(manga);
        var mangaMessage = url;
        await BotClient.SendTextMessageAsync(chatId, mangaMessage);
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