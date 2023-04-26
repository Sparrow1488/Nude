namespace Nude.API.Infrastructure.Abstractions;

public interface IServiceResult<TResult> : IServiceResult
{
    TResult? Result { get; set; }
}