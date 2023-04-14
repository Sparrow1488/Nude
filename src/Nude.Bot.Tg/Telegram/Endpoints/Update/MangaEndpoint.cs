using Microsoft.Extensions.DependencyInjection;
using Nude.API.Contracts.Formats.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Infrastructure.Utility;
using Nude.API.Models.Formats;
using Nude.API.Models.Messages;
using Nude.API.Models.Users;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Nude.Data.Infrastructure.Contexts;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class MangaEndpoint : TelegramUpdateEndpoint
{
    private readonly INudeClient _nudeClient;
    private readonly IMessagesStore _messages;
    private readonly IServiceProvider _services;

    public MangaEndpoint(
        INudeClient nudeClient,
        IMessagesStore messages,
        IServiceProvider services)
    {
        _nudeClient = nudeClient;
        _messages = messages;
        _services = services;
    }
    
    public override async Task HandleAsync()
    {
        var mangaResponse = await _nudeClient.FindMangaByUrlAsync(MessageText, FormatType.Telegraph);

        if (mangaResponse is not null)
        {
            var manga = mangaResponse.Value;
            var tghFormat = (TelegraphFormatResponse) manga.Formats.First(x => x is TelegraphFormatResponse);
            await MessageAsync(await _messages.GetReadMangaMessageAsync(tghFormat.Url));
            return;
        }

        var request = new ContentTicketRequest { SourceUrl = MessageText };
        var response = await _nudeClient.CreateContentTicket(request);
        
        var botMessage = await MessageAsync(new MessageItem("Нужно немного подождать", ParseMode.MarkdownV2));
        
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BotDbContext>();
        
        await context.AddAsync(new UserMessage
        {
            ChatId = ChatId,
            MessageId = botMessage.MessageId, 
            TicketId = response!.Value.Id,
            ContentKey = response.Value.ContentKey,
            Owner = UserSession.User
        });
        await context.SaveChangesAsync();
    }

    public override bool CanHandle() => ContentAware.IsSealingAvailable(Update.Message?.Text ?? "");
}














