using System.Text;
using Newtonsoft.Json;
using Nude.API.Infrastructure.Configurations.Json;
using Nude.API.Infrastructure.Exceptions;
using Nude.API.Services.WebHooks.Results;

namespace Nude.API.Services.WebHooks;

public class WebHookService : IWebHookService
{
    private readonly ILogger<WebHookService> _logger;
    private readonly JsonSerializerSettings _settings;

    public WebHookService(ILogger<WebHookService> logger)
    {
        _logger = logger;
        _settings = JsonSettingsProvider.CreateDefault();
    }
    
    public async Task<SendingResult> SendAsync<T>(string callbackUrl, T content)
    {
        Exception? exception = null;
        
        _logger.LogInformation("Send ticket processed request for url {url}", callbackUrl);
        
        if (Uri.TryCreate(callbackUrl, UriKind.Absolute, out _))
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(50);

            var json = JsonConvert.SerializeObject(content, _settings);
            using var request = new StringContent(json, Encoding.UTF8, "application/json");

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

        var statusCode = (int) response.StatusCode;
        if (statusCode is < 200 or >= 300)
            return new BadClientResponseException($"Get {response.StatusCode} status code in web hook");

        return null;
    }
}