namespace Nude.API.Services.WebHooks.Results;

public class SendingResult
{
    public bool IsSuccess { get; set; }
    public Exception? Exception { get; set; }
}