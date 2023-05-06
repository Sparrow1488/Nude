using Nude.API.Models.Formats;

namespace Nude.API.Services.Mangas;

public class SearchMangaFilter
{
    public FormatType? Format { get; set; } = null;
    public int[] ExceptIds { get; set; } = Array.Empty<int>();
}