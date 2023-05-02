using Nude.API.Infrastructure.Abstractions;
using Nude.API.Models.Mangas;

namespace Nude.API.Services.Mangas.Results;

public class MangaCreationResult : ServiceResult<MangaEntry>
{
    public MangaCreationResult(Exception exception) : base(exception)
    {
    }

    public MangaCreationResult(MangaEntry result) : base(result)
    {
    }
}