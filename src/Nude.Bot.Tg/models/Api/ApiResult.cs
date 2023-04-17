using Nude.API.Contracts.Errors.Responses;

namespace Nude.Bot.Tg.Models.Api;

public class ApiResult<TResult>
{
    public ApiResult(TResult? result, ErrorResponse? error, int status)
    {
        Result = result;
        Error = error;
        Status = status;
    }

    public int Status { get; }
    public TResult? Result { get; }
    public ErrorResponse? Error { get; }

    public bool IsSuccess => Error == null;
    public string? Message => Error?.Message;
    public TResult ResultValue => Result!;
    public ErrorResponse ErrorValue => Error!.Value;
}