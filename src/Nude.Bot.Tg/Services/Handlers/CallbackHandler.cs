using Nude.API.Contracts.Formats.Responses;
using Nude.API.Infrastructure.Constants;
using Nude.API.Models.Messages;
using Nude.API.Models.Notifications;
using Nude.API.Models.Notifications.Details;
using Nude.API.Models.Tickets.States;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Services.Messages.Service;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Users;
using Nude.Bot.Tg.Services.Utils;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Services.Handlers;

public class CallbackHandler : ICallbackHandler
{
    private readonly INudeClient _client;
    private readonly ITelegramBotClient _bot;
    private readonly IMessagesStore _messageStore;
    private readonly IMessageService _messageService;

    public CallbackHandler(
        INudeClient client,
        ITelegramBotClient bot,
        IMessagesStore messageStore,
        IMessageService messageService)
    {
        _client = client;
        _bot = bot;
        _messageStore = messageStore;
        _messageService = messageService;
    }
    
    public async Task HandleAsync(Notification notification)
    {
        var contentKey = (notification.Details as ContentNotificationDetails)!.ContentKey;
        var messages = await _messageService.FindByContentKeyAsync(contentKey);

        // Прогресс форматирования
        if (notification.Details is FormattingProgressDetails progress)
        {
            await EditMessagesAsync(messages, $"Загрузка {progress.CurrentImage} из {progress.TotalImages}");
            return;
        }

        if (notification.Details is ContentTicketChangedDetails ticketDetails)
        {
            switch (ticketDetails.Status)
            {
                case ReceiveStatus.Started:
                    await EditMessagesAsync(messages, "Пиздим содержимое с сайта");
                    break;
                case ReceiveStatus.Success:
                    var readyMangaResult = await _client.FindMangaByContentKeyAsync(contentKey);
                    if (readyMangaResult.ResultValue.Images.Count > ContentLimits.MaxFormatImagesCount)
                    {
                        await EditMessagesAsync(messages, "Манга слишком большая! Попробуйте загрузить мангу, в которой меньше 45 изображений", ParseMode.Html);
                        await _messageService.RemoveRangeAsync(messages);
                    }
                    else
                    {
                        await EditMessagesAsync(messages, "Все спиздили, ожидайте своей очереди");
                    }
                    break;
                case ReceiveStatus.Failed:
                    await EditMessagesAsync(messages, "Не удалось получить содержимое по запросу");
                    await _messageService.RemoveRangeAsync(messages);
                    break;
            }
        }

        // Все готово, лови ссылку
        if (notification.Details is FormattingStatusDetails formatDetails)
        {
            if (formatDetails.Status == FormattingStatus.Success)
            {
                var manga = await _client.FindMangaByContentKeyAsync(formatDetails.ContentKey);
                var tgh = manga.ResultValue.Formats.First(x => x is TelegraphFormatResponse);
                var url = ((TelegraphFormatResponse) tgh).Url;
                
                var message = await _messageStore.GetReadMangaMessageAsync(url);
                await EditMessagesAsync(messages, message.Text, message.ParseMode);

                await _messageService.RemoveRangeAsync(messages);
            }
            else
            {
                await EditMessagesAsync(messages, "Начали конвертировать мангу");
            }
        }
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
}