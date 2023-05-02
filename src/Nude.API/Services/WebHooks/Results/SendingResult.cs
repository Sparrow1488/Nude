using Nude.API.Infrastructure.Abstractions;

namespace Nude.API.Services.WebHooks.Results;

public class SendingResult : IServiceResult
{
    public bool IsSuccess => Exception is not null;
    public Exception? Exception { get; set; }
}