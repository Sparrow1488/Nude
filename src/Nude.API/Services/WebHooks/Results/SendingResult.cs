using Nude.API.Infrastructure.Abstractions;

namespace Nude.API.Services.WebHooks.Results;

public class SendingResult : IServiceResult
{
    public bool IsSuccess { get; set; }
    public Exception? Exception { get; set; }
}