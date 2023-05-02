using System.Text;
using Newtonsoft.Json;
using Nude.API.Infrastructure.Configurations.Json;
using Nude.API.Infrastructure.Exceptions.Internal;
using Nude.API.Services.WebHooks.Results;

namespace Nude.API.Services.WebHooks;

public class WebHookService : IWebHookService
{
    private readonly ILogger<WebHookService> _logger;
    private readonly JsonSerializerSettings _settings;
    private readonly HttpClient _client;

    public WebHookService(
        IHttpClientFactory clientFactory, 
        ILogger<WebHookService> logger)
    {
        _logger = logger;
        _settings = JsonSettingsProvider.CreateDefault();

        _client = clientFactory.CreateClient("web_hook");
    }
    
    public async Task<SendingResult> SendAsync<T>(string callbackUrl, T content)
    {
        Exception? exception = null;
        
        _logger.LogInformation("Send ticket processed request for url {url}", callbackUrl);
        
        if (Uri.TryCreate(callbackUrl, UriKind.Absolute, out _))
        {
            var json = JsonConvert.SerializeObject(content, _settings);
            using var request = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await PostAsync(_client, callbackUrl, request);
            _logger.LogInformation("Callback response status '{status}'", response?.StatusCode);

            exception = CreateExceptionByResponse(response);
        }
        else
        {
            _logger.LogWarning("Cannot create uri");
        }

        return new SendingResult { Exception = exception };
    }
    
    private async Task<HttpResponseMessage?> PostAsync(HttpClient client, string url, HttpContent content)
    {
        HttpResponseMessage? message = null;
        try
        {
            message = await client.PostAsync(url, content);
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
            return new BadServerResponseException($"Get {response.StatusCode} status code in web hook");

        return null;
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}