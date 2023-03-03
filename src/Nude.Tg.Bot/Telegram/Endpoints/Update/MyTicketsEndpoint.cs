using System.Text;
using Nude.Tg.Bot.Services.Convert;
using Nude.Tg.Bot.Telegram.Endpoints.Base;

namespace Nude.Tg.Bot.Telegram.Endpoints.Update;

public class MyTicketsEndpoint : TelegramUpdateEndpoint
{
    private readonly IConvertTicketsService _ticketsService;

    public MyTicketsEndpoint(IConvertTicketsService ticketsService)
    {
        _ticketsService = ticketsService;
    }
    
    public override async Task HandleAsync()
    {
        var tickets = (await _ticketsService.GetAllByChatIdAsync(ChatId)).ToList();
        var builder = new StringBuilder();
        builder.AppendLine("Всего " + tickets.Count);
        for (var i = 0; i < tickets.Count; i++)
        {
            var ticket = tickets[i];
            builder.AppendLine($"{i + 1}) Запрос {ticket.Id} со статусом {ticket.Status}");
        }

        await MessageAsync(builder.ToString());
    }

    public override bool CanHandle()
    {
        return MessageText == "/requests";
    }
}