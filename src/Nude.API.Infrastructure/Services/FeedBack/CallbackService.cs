using Microsoft.Extensions.Logging;
using Nude.Models.Tickets.Parsing;

namespace Nude.API.Infrastructure.Services.FeedBack;

public class CallbackService : IFeedBackService
{
    private readonly ILogger<CallbackService> _logger;

    public CallbackService(ILogger<CallbackService> logger)
    {
        _logger = logger;
    }
    
    public async Task SendAsync(ParsingTicket ticket, FeedBackInfo info)
    {
        _logger.LogInformation("Send ticket processed request for url {url}", info.CallbackUrl);
        
        if (Uri.TryCreate(info.CallbackUrl, UriKind.Absolute, out var url))
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(8);

            var query = $"?ticket_id={ticket.Id}&status={ticket.Status}";
            var requestUrl = url + query;
            var content = new StringContent(string.Empty);
            
            var response = await client.PostAsync(requestUrl, content);
            _logger.LogInformation("Callback response status '{status}'", response.StatusCode);
        }
        else
        {
            _logger.LogWarning("Cannot create uri");
        }
    }
}