using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nude.Bot.Tg.Services.Messages.Store;
using Nude.Bot.Tg.Services.Utils;
using Nude.Data.Infrastructure.Contexts;
using Nude.Models.Tickets.Converting;
using Nude.Models.Tickets.Parsing;
using Telegram.Bot;

namespace Nude.Bot.Tg.Http.Routes;

public class CallbackRoute
{
    private readonly BotDbContext _context;
    private readonly ITelegramBotClient _tgBot;
    private readonly IMessagesStore _messagesStore;
    private readonly ILogger<CallbackRoute> _logger;

    public CallbackRoute(
        BotDbContext context,
        ITelegramBotClient tgBot,
        IMessagesStore messagesStore,
        ILogger<CallbackRoute> logger)
    {
        _context = context;
        _tgBot = tgBot;
        _messagesStore = messagesStore;
        _logger = logger;
    }
    
    public async Task OnCallbackAsync(int ticketId, ParsingStatus status)
    {
        _logger.LogInformation(
            "Callback received parsing ticket ({id}), status:{status}",
            ticketId,
            status.ToString());
        
        var ticket = await _context.ConvertingTickets
            .FirstOrDefaultAsync(x => x.ParsingId == ticketId);
        
        if (ticket is null)
        {
            _logger.LogError("Converting Ticket not found");
            return;
        }

        ticket.Status = status == ParsingStatus.Success 
            ? ConvertingStatus.ConvertWaiting 
            : ConvertingStatus.Failed;

        await _context.SaveChangesAsync();

        if (status == ParsingStatus.Failed)
        {
            await BotUtils.MessageAsync(
                _tgBot, 
                ticket.ChatId, 
                await _messagesStore.GetCallbackFailedMessageAsync()
            );
        }
    }
}