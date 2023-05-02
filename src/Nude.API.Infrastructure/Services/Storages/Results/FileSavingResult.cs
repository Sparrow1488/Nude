using Nude.API.Infrastructure.Abstractions;

namespace Nude.API.Infrastructure.Services.Storages.Results;

public class FileSavingResult : IServiceResult
{
    public string? Url { get; set; }
    public bool IsSuccess => Exception is null;
    public Exception? Exception { get; set; }
}