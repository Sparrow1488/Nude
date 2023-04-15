using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nude.API.Contracts.Formats.Responses;
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
        var contentKey = (subject.Details as ContentNotificationDetails)!.ContentKey;
        var messages = await GetUserMessagesAsync(contentKey);
        
        // Прогресс форматирования
        if (subject.Details is FormattingProgressDetails progress)
        {
            await EditMessagesAsync(messages, $"Загрузка {progress.CurrentImage} из {progress.TotalImages}");
            return;
        }

        if (subject.Details is ContentTicketChangedDetails ticketDetails)
        {
            switch (ticketDetails.Status)
            {
                case ReceiveStatus.Started:
                    await EditMessagesAsync(messages, $"Пиздим мангу");
                    break;
                case ReceiveStatus.Success:
                    await EditMessagesAsync(messages, $"Спиздили мангу");
                    break;
                case ReceiveStatus.Failed:
                    await EditMessagesAsync(messages, "Не удалось получить содержимое по запросу");
                    await DeleteMessagesAsync(messages);
                    break;
            }
        }

        // Все готово, лови ссылку
        if (subject.Details is FormattingStatusDetails formatDetails)
        {
            if (formatDetails.Status == FormattingStatus.Success)
            {
                var manga = await _client.FindMangaByContentKeyAsync(formatDetails.ContentKey);
                var tgh = manga!.Result.Formats.First(x => x is TelegraphFormatResponse);
                var url = ((TelegraphFormatResponse) tgh).Url;
                await EditMessagesAsync(messages, url, ParseMode.Html);

                await DeleteMessagesAsync(messages);
            }
            else
            {
                await EditMessagesAsync(messages, $"Начали конвертировать мангу");
            }
        }
    }

    private Task<List<UserMessage>> GetUserMessagesAsync(string contentKey)
    {
        return _context.Messages
            .Where(x => x.ContentKey == contentKey)
            .ToListAsync();
    }

    private async Task EditMessagesAsync(IEnumerable<UserMessage> messages, string text, ParseMode mode = ParseMode.MarkdownV2)
    {
        foreach (var message in messages)
        {
            var messageItem = new MessageItem(text, mode);
            var messageId = int.Parse(message.MessageId.ToString());
            await BotUtils.EditMessageAsync(_bot, message.ChatId, messageId, messageItem);
        }
    }

    private async Task DeleteMessagesAsync(IEnumerable<UserMessage> messages)
    {
        if (messages.Any())
        {
            _context.RemoveRange(messages);
            await _context.SaveChangesAsync();
        }
    }
}