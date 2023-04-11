namespace Nude.API.Infrastructure.Abstractions;

public interface IServiceResult
{
    bool IsSuccess { get; set; }
    Exception? Exception { get; set; }
}