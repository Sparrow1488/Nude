using Nude.API.Infrastructure.Abstractions;

namespace Nude.API.Infrastructure.Services.Storages.Results;

public class FileSavingResult : IServiceResult
{
    public string? Url { get; set; }
    public bool IsSuccess { get; set; }
    public Exception? Exception { get; set; }
}