using System.Text;
using Nude.Bot.Tg.Services.Convert;
using Nude.Bot.Tg.Telegram.Endpoints.Base;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update;

public class MyTicketsEndpoint : TelegramUpdateCommandEndpoint
{
    private readonly IConvertTicketsService _ticketsService;

    public MyTicketsEndpoint(IConvertTicketsService ticketsService) : base("/requests")
    {
        _ticketsService = ticketsService;
    }
    
    public override async Task HandleAsync()
    {
        var tickets = (await _ticketsService.GetAllByChatIdAsync(ChatId)).ToList();
        var builder = new StringBuilder();
        builder.AppendLine("Всего Ваших запросов: " + tickets.Count);
        for (var i = 0; i < tickets.Count; i++)
        {
            var ticket = tickets[i];
            builder.AppendLine($"{i + 1}) Запрос {ticket.Id} со статусом {ticket.Status}");
        }

        await MessageAsync(builder.ToString());
    }
}