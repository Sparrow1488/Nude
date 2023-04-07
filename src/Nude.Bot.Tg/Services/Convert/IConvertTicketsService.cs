using Nude.Models.Tickets.Converting;

namespace Nude.Bot.Tg.Services.Convert;

public interface IConvertTicketsService
{
    Task<ConvertingTicket?> GetByParsingIdAsync(int parsingId);
    
    Task<ConvertingTicket> CreateAsync(
        int parsingId, 
        long chatId, 
        ConvertingStatus status = ConvertingStatus.ParseWaiting);
    
    Task<IEnumerable<ConvertingTicket>> GetAllByChatIdAsync(long chatId);
}