namespace Nude.API.Infrastructure.Abstractions;

public abstract class ServiceResult<TResult> : IServiceResult<TResult>
{
    public ServiceResult(Exception exception)
    {
        Exception = exception;
    }

    public ServiceResult(TResult result)
    {
        Result = result;
    }

    public bool IsSuccess => Exception == null;
    public Exception? Exception { get; set; }
    public TResult? Result { get; set; }
}