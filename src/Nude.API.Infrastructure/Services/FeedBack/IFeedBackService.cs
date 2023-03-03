using Nude.Models.Tickets;
using Nude.Models.Tickets.Parsing;

namespace Nude.API.Infrastructure.Services.FeedBack;

public interface IFeedBackService
{
    Task SendAsync(ParsingTicket ticket, FeedBackInfo info);
}