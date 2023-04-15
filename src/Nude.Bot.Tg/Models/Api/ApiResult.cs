using Nude.API.Contracts.Errors.Responses;

namespace Nude.Bot.Tg.Models.Api;

public class ApiResult<TResult>
where TResult : struct
{
    public ApiResult(TResult? result, ErrorResponse? error)
    {
        Result = result;
        Error = error;
    }
    
    public TResult? Result { get; }
    public ErrorResponse? Error { get; }

    public bool IsSuccess => Error == null;
    public string? Message => Error?.Message;
    public TResult ResultValue => Result!.Value;
    public ErrorResponse ErrorValue => Error!.Value;

}