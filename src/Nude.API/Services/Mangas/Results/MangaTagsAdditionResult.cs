using Nude.API.Infrastructure.Abstractions;

namespace Nude.API.Services.Mangas.Results;

public class MangaTagsAdditionResult : IServiceResult
{
    public bool IsSuccess => Exception is not null;
    public Exception? Exception { get; set; }
}