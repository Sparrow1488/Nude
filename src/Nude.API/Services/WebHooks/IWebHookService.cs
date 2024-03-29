using Nude.API.Services.WebHooks.Results;

namespace Nude.API.Services.WebHooks;

public interface IWebHookService : IDisposable
{
    Task<SendingResult> SendAsync<T>(string callbackUrl, T content);
}