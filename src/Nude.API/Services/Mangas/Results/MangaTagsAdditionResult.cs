using Nude.API.Infrastructure.Abstractions;

namespace Nude.API.Services.Mangas.Results;

public class MangaTagsAdditionResult : IServiceResult
{
    public bool IsSuccess { get; set; }
    public Exception? Exception { get; set; }
}