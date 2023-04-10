using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nude.API.Contracts.Formats.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Models.Messages;
using Nude.API.Models.Notifications;
using Nude.API.Models.Notifications.Details;
using Nude.API.Models.Tickets.States;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Utils;
using Nude.Data.Infrastructure.Contexts;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Http.Routes;

public class CallbackRoute
{
    private readonly BotDbContext _context;
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
        BotDbContext context,
        ILogger<CallbackRoute> logger)
    {
        _context = context;
        _messagesStore = messagesStore;
        _client = client;
        _configuration = configuration;
        _logger = logger;
        _bot = bot;
    }
    
    public async Task OnCallbackAsync(Notification subject)
    {
        // Прогресс форматирования
        if (subject.Details is ContentFormatProgressDetails progress)
        {
            var message = await GetUserMessageAsync(progress.ContentKey);
            await EditMessageAsync(message, $"Загрузка {progress.CurrentImage} из {progress.TotalImages}");
            return;
        }

        // Содержимое спизжено, если Success, то создаем ContentFormatTicket
        if (subject.Details is ContentTicketStatusChangedDetails ticketDetails)
        {
            var message = await GetUserMessageAsync(ticketDetails.ContentKey);

            if (ticketDetails.Status == ReceiveStatus.Success)
            {
                await EditMessageAsync(message, $"Пиздим мангу");
            }
            else
            {
                await EditMessageAsync(message, "Не удалось получить содержимое по запросу");
            }
        }

        // Все готово, лови ссылку
        if (subject.Details is ContentFormatReadyDetails formatDetails)
        {
            var message = await GetUserMessageAsync(formatDetails.ContentKey);
            if (formatDetails.Status == FormattingStatus.Success)
            {
                var manga = await _client.GetMangaByContentKeyAsync(formatDetails.ContentKey);
                var tgh = manga!.Value.Formats.First(x => x is TelegraphContentResponse);
                var url = ((TelegraphContentResponse) tgh).Url;
                await EditMessageAsync(message, url, ParseMode.Html);
            }
            else
            {
                await EditMessageAsync(message, $"Начали конвертировать мангу");
            }
        }
    }

    private Task<UserMessages?> GetUserMessageAsync(string contentKey)
    {
        return _context.Messages
            .FirstOrDefaultAsync(x => x.ContentKey == contentKey);
    }

    private async Task EditMessageAsync(UserMessages? message, string text, ParseMode mode = ParseMode.MarkdownV2)
    {
        if (message != null)
        {
            var messageItem = new MessageItem(text, mode);
            var messageId = int.Parse(message.MessageId.ToString());
            await BotUtils.EditMessageAsync(_bot, message.ChatId, messageId, messageItem);
        }
    }
}