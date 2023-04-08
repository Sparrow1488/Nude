using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nude.API.Infrastructure.Exceptions;
using Nude.API.Infrastructure.Services.WebHooks.Results;

namespace Nude.API.Infrastructure.Services.WebHooks;

public class WebHookService : IWebHookService
{
    private readonly ILogger<WebHookService> _logger;

    public WebHookService(ILogger<WebHookService> logger)
    {
        _logger = logger;
    }
    
    public async Task<SendingResult> SendAsync<T>(string callbackUrl, T content)
    {
        Exception? exception = null;
        
        _logger.LogInformation("Send ticket processed request for url {url}", callbackUrl);
        
        if (Uri.TryCreate(callbackUrl, UriKind.Absolute, out var url))
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var json = JsonConvert.SerializeObject(content, Formatting.Indented);
            using var request = new StringContent(json);

            var response = await PostAsync(client, callbackUrl, request);
            _logger.LogInformation("Callback response status '{status}'", response?.StatusCode);

            exception = CreateExceptionByResponse(response);
        }
        else
        {
            _logger.LogWarning("Cannot create uri");
        }

        return new SendingResult
        {
            IsSuccess = exception == null,
            Exception = exception
        };
    }
    
    private async Task<HttpResponseMessage?> PostAsync(HttpClient client, string url, HttpContent content)
    {
        HttpResponseMessage? message = null;
        try
        {
            message = await client.PostAsync(url, content)!;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex.Message);
        }

        return message;
    }

    private Exception? CreateExceptionByResponse(HttpResponseMessage? response)
    {
        if (response == null)
            return new NullReferenceException("Http response null result");

        if (!response.StatusCode.ToString().StartsWith('2'))
            return new BadClientResponseException($"Get {response.StatusCode} status code in web hook");

        return null;
    }
}