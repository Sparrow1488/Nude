using Nude.API.Infrastructure.Abstractions;
using Nude.API.Models.Mangas;

namespace Nude.API.Services.Mangas.Results;

public class MangaCreationResult : IServiceResult<MangaEntry>
{
    public bool IsSuccess { get; set; }
    public MangaEntry? Result { get; set; }
    public Exception? Exception { get; set; }
}