using Nude.API.Infrastructure.Services.WebHooks.Results;

namespace Nude.API.Infrastructure.Services.WebHooks;

public interface IWebHookService
{
    Task<SendingResult> SendAsync<T>(string callbackUrl, T content);
}