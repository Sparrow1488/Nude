using Nude.Models.Tickets;

namespace Nude.API.Infrastructure.Services.FeedBack;

public interface IFeedBackService
{
    Task SendAsync(ParsingTicket ticket, FeedBackInfo info);
}