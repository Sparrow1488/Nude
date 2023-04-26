using Nude.API.Infrastructure.Abstractions;

namespace Nude.API.Services.Notifications.Results;

public class NotificationResult : IServiceResult
{
    public bool IsSuccess { get; set; }
    public Exception? Exception { get; set; }
}
