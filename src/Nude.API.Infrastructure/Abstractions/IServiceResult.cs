namespace Nude.API.Infrastructure.Abstractions;

public interface IServiceResult
{
    bool IsSuccess { get; }
    Exception? Exception { get; }
}