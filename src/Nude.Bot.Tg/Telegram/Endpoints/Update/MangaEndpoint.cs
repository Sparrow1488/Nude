using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nude.API.Contracts.Formats.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Infrastructure.Constants;
using Nude.API.Models.Formats;
using Nude.API.Models.Messages;
using Nude.API.Models.Tickets;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Nude.Data.Infrastructure.Contexts;
using Telegram.Bot.Types.Enums;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class MangaEndpoint : TelegramUpdateEndpoint
{
    private readonly ILogger<MangaEndpoint> _logger;
    private readonly INudeClient _nudeClient;
    private readonly IConfiguration _configuration;
    private readonly IMessagesStore _messages;
    private readonly BotDbContext _context;
    private readonly IServiceProvider _services;

    public MangaEndpoint(
        INudeClient nudeClient,
        IConfiguration configuration,
        IMessagesStore messages,
        BotDbContext context,
        IServiceProvider services,
        ILogger<MangaEndpoint> logger)
    {
        _logger = logger;
        _nudeClient = nudeClient;
        _configuration = configuration;
        _messages = messages;
        _context = context;
        _services = services;
    }
    
    public override async Task HandleAsync()
    {
        var mangaResponse = await _nudeClient.GetMangaByUrlAsync(MessageText, FormatType.Telegraph);

        if (mangaResponse is not null)
        {
            var manga = mangaResponse.Value;
            var tghFormat = (TelegraphContentResponse) manga.Formats.First(x => x is TelegraphContentResponse);
            await MessageAsync(await _messages.GetReadMangaMessageAsync(tghFormat.Url));
            return;
        }

        var request = new ContentTicketRequest { SourceUrl = MessageText };
        var response = await _nudeClient.CreateContentTicket(request);
        
        var botMessage = await MessageAsync(new MessageItem("Нужно немного подождать", ParseMode.MarkdownV2));
        
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BotDbContext>();
        
        await context.AddAsync(new UserMessages
        {
            ChatId = ChatId,
            MessageId = botMessage.MessageId, 
            UserId = Update.Message!.From!.Id,
            TicketId = response!.Value.Id,
            TicketType = nameof(ContentTicket),
            ContentKey = response.Value.ContentKey
        });
        await context.SaveChangesAsync();
    }

    public override bool CanHandle() => AvailableSources.IsAvailable(Update.Message?.Text ?? "");
}














