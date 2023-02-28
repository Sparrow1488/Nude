using Nude.API.Data.Contexts;
using Nude.Models.Tickets;

namespace Nude.Tg.Bot.Routes;

public class CallbackRoute
{
    public CallbackRoute(BotDbContext context)
    {
        
    }
    
    public Task OnCallbackAsync(int ticketId, ParsingStatus status)
    {
        // TODO: add task to bg convert service
        return Task.CompletedTask;
    }
}