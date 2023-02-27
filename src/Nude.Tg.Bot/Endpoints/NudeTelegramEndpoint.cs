using Newtonsoft.Json;
using Nude.Tg.Bot.Clients;
using Telegram.Bot;

namespace Nude.Tg.Bot.Endpoints;

public class NudeTelegramEndpoint : TelegramUpdateEndpoint
{
    public override async Task HandleAsync()
    {
        var client = new NudeClient();
        var messageText = Update.Message!.Text ?? "";
        var chatId = Update.Message!.Chat.Id;
        var manga = await client.GetMangaByUrlAsync(messageText);
            
        if (manga is null)
        {
            await BotClient.SendTextMessageAsync(chatId, "Нам не удалось найти эту мангу у себя. Но не переживайте, когда она появится мы сообщим Вам об этом!");
            var parsingResponse = await client.CreateParsingRequestAsync(messageText, "");
            var parsingMessage = $"Id:{parsingResponse.UniqueId}\nStatus: {parsingResponse.Status}\nIsExists: {parsingResponse.IsAlreadyExists}";
            await BotClient.SendTextMessageAsync(chatId, parsingMessage);
            return;
        }

        var mangaMessage = $"Title: {manga?.Title}\nImages: {manga?.Images.Count}\nAuthor: {manga?.Author}";
        await BotClient.SendTextMessageAsync(chatId, mangaMessage);
    }

    public override bool CanHandle()
    {
        return Update.Message?.Text?.Contains("nude-moon.org") ?? false;
    }
}