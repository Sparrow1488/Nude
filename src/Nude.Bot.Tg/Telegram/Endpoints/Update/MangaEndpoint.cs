using Nude.API.Contracts.Formats.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Infrastructure.Utility;
using Nude.API.Models.Formats;
using Nude.API.Models.Messages;
using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Nude.Data.Infrastructure.Contexts;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class MangaEndpoint : TelegramUpdateEndpoint
{
    private readonly BotDbContext _context;
    private readonly INudeClient _nudeClient;
    private readonly IMessagesStore _messages;
    private readonly IServiceProvider _services;

    public MangaEndpoint(
        BotDbContext context,
        INudeClient nudeClient,
        IMessagesStore messages,
        IServiceProvider services)
    {
        _context = context;
        _nudeClient = nudeClient;
        _messages = messages;
        _services = services;
    }
    
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
        
            await _context.AddAsync(new UserMessage
            {
                ChatId = ChatId,
                MessageId = botMessage.MessageId, 
                TicketId = ticketResult.ResultValue.Id,
                ContentKey = ticketResult.ResultValue.ContentKey,
                OwnerId = UserSession.User.Id
            });
            await _context.SaveChangesAsync();
        }
        else
        {
            var badMessage = ticketResult.Status + ": " + ticketResult.Message;
            await MessageAsync(new MessageItem(badMessage, ParseMode.Html));
        }
    }

    public override bool CanHandle() => ContentAware.IsSealingAvailable(Update.Message?.Text ?? "");
}














