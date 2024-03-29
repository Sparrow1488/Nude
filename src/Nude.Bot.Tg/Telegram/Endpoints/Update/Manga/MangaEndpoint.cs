using Nude.API.Contracts.Formats.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Infrastructure.Utility;
using Nude.API.Models.Formats;
using Nude.API.Models.Messages.Details;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Services.Messages.Service;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Manga;

public class MangaEndpoint : TelegramUpdateEndpoint
{
    private readonly INudeClient _nudeClient;
    private readonly IMessagesStore _messages;
    private readonly IMessageService _messageService;

    public MangaEndpoint(
        INudeClient nudeClient,
        IMessagesStore messages,
        IMessageService messageService)
    {
        _nudeClient = nudeClient;
        _messages = messages;
        _messageService = messageService;
    }
    
    public override bool CanHandle() => ContentAware.IsSealingAvailable(MessageText);
    
    public override async Task HandleAsync()
    {
        var mangaResult = await _nudeClient.FindMangaByUrlAsync(MessageText, FormatType.Telegraph);

        if (mangaResult.IsSuccess)
        {
            var manga = mangaResult.ResultValue;
            var tghFormat = (TelegraphFormatResponse) manga.Formats.First(
                x => x is TelegraphFormatResponse
            );
            
            await MessageAsync(await _messages.GetReadMangaMessageAsync(tghFormat.Url));
            return;
        }

        var authorizedClient = _nudeClient.AuthorizeClient(UserSession);
        var request = new ContentTicketRequest { SourceUrl = MessageText };
        var ticketResult = await authorizedClient.CreateContentTicketAsync(request);

        if (ticketResult.IsSuccess)
        {
            var botMessage = await MessageAsync(new MessageItem("Нужно немного подождать", ParseMode.MarkdownV2));
        
            var details = new ContentTicketDetails
            {
                TicketId = ticketResult.ResultValue.Id,
                ContentKey = ticketResult.ResultValue.ContentKey
            };
            await _messageService.CreateAsync(
                ChatId, 
                botMessage.MessageId, 
                details, 
                UserSession.User
            );
        }
        else
        {
            var badMessage = await _messages.GetErrorResponseMessageAsync(ticketResult.ErrorValue);
            await MessageAsync(badMessage);
        }
    }
}














