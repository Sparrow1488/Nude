using Nude.API.Models.Mangas;

namespace Nude.API.Services.Mangas.Results;

public class MangaCreationResult
{
    public bool IsSuccess { get; set; }
    public MangaEntry? Entry { get; set; }
    public Exception? Exception { get; set; }
}