using Nude.API.Contracts.Errors.Responses;

namespace Nude.Bot.Tg.models.Api;

public class ApiResult<T>
{
    public T? Result { get; set; }
    public bool IsSuccess => ErrorResponse == null;
    public string? Message { get; set; }
    public ErrorResponse? ErrorResponse { get; set; }
}