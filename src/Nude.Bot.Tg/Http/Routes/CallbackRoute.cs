using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Models.Formats;
using Nude.API.Models.Notifications;
using Nude.API.Models.Tickets;
using Nude.Bot.Tg.Clients.Nude;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Utils;
using Nude.Data.Infrastructure.Contexts;
using Nude.Models.Tickets.Converting;
using Nude.Models.Tickets.Parsing;
using Telegram.Bot;

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
    
    public async Task OnCallbackAsync(NotificationSubject subject)
    {
        _logger.LogInformation(
            "Callback received parsing ticket ({id}), status:{status}",
            subject.EntityId,
            subject.Status);
        
        var entity = await  _context.Messages.FirstOrDefaultAsync(x => 
            x.Id == int.Parse(subject.EntityId) && x.TicketType.Equals(subject.EntityType));

        if (subject.EntityType.Equals(nameof(ContentTicket)))
        {
            var callback = _configuration.GetRequiredSection("Http:BaseUrl") + "/callback";

            var request = new FormatTicketRequest
            {
                EntryId = subject.EntityId,
                EntryType = subject.EntityType,
                FormatType = FormatType.Telegraph,
                CallbackUrl = callback
            };
            var respons = await _client.CreateFormatTicket(request);
            await _bot.SendTextMessageAsync(entity!.ChatId, "Форматируем мангу");
        }

        if (subject.EntityType.Equals(nameof(ContentFormatTicket)))
        {
            await _bot.SendTextMessageAsync(entity!.ChatId, "");
        }
        
    }
}