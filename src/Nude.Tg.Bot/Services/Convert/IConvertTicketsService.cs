using Nude.Models.Tickets.Converting;

namespace Nude.Tg.Bot.Services.Convert;

public interface IConvertTicketsService
{
    Task<ConvertingTicket?> GetByParsingIdAsync(int parsingId);
    
    Task<ConvertingTicket> CreateAsync(
        int parsingId, 
        long chatId, 
        ConvertingStatus status = ConvertingStatus.ParseWaiting);
    
    Task<IEnumerable<ConvertingTicket>> GetAllByChatIdAsync(long chatId);
}