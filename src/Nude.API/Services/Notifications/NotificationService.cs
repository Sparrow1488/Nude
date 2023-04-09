using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Notifications;
using Nude.API.Services.Notifications.Results;
using Nude.API.Services.WebHooks;
using Nude.Data.Infrastructure.Contexts;

namespace Nude.API.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly FixedAppDbContext _context;
    private readonly IWebHookService _webHookService;

    public NotificationService(
        FixedAppDbContext context,
        IWebHookService webHookService)
    {
        _context = context;
        _webHookService = webHookService;
    }
    
    public async Task<NotificationResult> NotifyAsync(NotificationSubject subject)
    {
        var servers = await _context.Servers
            .ToListAsync();

        var callbackServer = servers.Where(x => x.NotificationsCallbackUrl != null);
        foreach (var server in callbackServer)
        {
            var result = await _webHookService.SendAsync(server.NotificationsCallbackUrl!, subject);
            return CreateResult(result.IsSuccess, result.Exception);
        }

        return CreateResult(true, null);
    }

    private static NotificationResult CreateResult(bool success, Exception? exception)
    {
        return new NotificationResult
        {
            IsSuccess = success,
            Exception = exception
        };
    }
}