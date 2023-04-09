using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nude.API.Contracts.Formats.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Models.Messages;
using Nude.API.Models.Notifications;
using Nude.API.Models.Notifications.Details;
using Nude.API.Models.Tickets;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Utils;
using Nude.Data.Infrastructure.Contexts;
using Nude.Models.Tickets.Converting;
using Nude.Models.Tickets.Parsing;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Http.Routes;

public class CallbackRoute
{
    private readonly FixedBotDbContext _context;
    private readonly IMessagesStore _messagesStore;
    private readonly ILogger<CallbackRoute> _logger;
    private readonly INudeClient _client;
    private readonly IConfiguration _configuration;
    private readonly ITelegramBotClient _bot;

    public CallbackRoute(
        IMessagesStore messagesStore,
        INudeClient client,
        IConfiguration configuration,
        ITelegramBotClient bot,
        FixedBotDbContext context,
        ILogger<CallbackRoute> logger)
    {
        _context = context;
        _messagesStore = messagesStore;
        _client = client;
        _configuration = configuration;
        _logger = logger;
        _bot = bot;
    }
    
    public async Task OnCallbackAsync(string userKey, NotificationSubject subject)
    {
        _logger.LogInformation(
            "Callback received parsing ticket ({id}), status:{status}",
            subject.EntityId,
            subject.Status);
        
        var userMessage = await  _context.Messages.FirstOrDefaultAsync(x => 
            x.UserKey == userKey);
        
        // Прогресс форматирования
        if (subject.Details is FormatProgressDetails progress)
        {
            // _logger.LogInformation($"MANGA UPLOADING PROGRESS {progress.CurrentImage}/{progress.TotalImages}");
            await EditMessageAsync(userMessage, $"Загрузка {progress.CurrentImage} из {progress.TotalImages}");
            return;
        }

        // Содержимое спизжено, если Success, то создаем ContentFormatTicket
        if (subject.EntityType.Equals(nameof(ContentTicket)))
        {
            if (subject.Status == "Success")
            {
                var callback = _configuration.GetRequiredSection("Http:BaseUrl").Value + "/callback?userKey=" + userMessage?.UserKey;

                var request = new FormatTicketRequest
                {
                    EntryId = subject.EntityId,
                    EntryType = nameof(MangaEntry),
                    FormatType = FormatType.Telegraph,
                    CallbackUrl = callback
                };
                var response = await _client.CreateFormatTicket(request);
            
                _logger.LogInformation("RECEIVE FORMAT TICKET RESPONSE, status: " + response.Value.Status);
                await EditMessageAsync(userMessage, $"Пиздим мангу");
            }
            else
            {
                await EditMessageAsync(userMessage, "Какая то ошибка");
            }
        }

        // Все готово, лови ссылку
        if (subject.EntityType.Equals(nameof(ContentFormatTicket)))
        {
            if (subject.Status == "Success")
            {
                var resultDetails = subject.Details as ContentFormattedResultDetails;
                var manga = await _client.GetMangaByIdAsync(int.Parse(resultDetails!.Id));
                var tgh = manga.Value.Formats.First(x => x is TelegraphContentResponse);
                await EditMessageAsync(userMessage, ((TelegraphContentResponse)tgh).Url);
            }
            else
            {
                await EditMessageAsync(userMessage, $"Начали конвертировать мангу");
            }
        }
    }

    private async Task EditMessageAsync(UserMessages? message, string text)
    {
        if (message != null)
        {
            var messageItem = new MessageItem(text, ParseMode.MarkdownV2);
            var messageId = int.Parse(message.MessageId.ToString());
            await BotUtils.EditMessageAsync(_bot, message.ChatId, messageId, messageItem);
        }
    }
}