using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Parsing.Responses;

namespace Nude.Tg.Bot.Clients.Nude;

public interface INudeClient
{
    Task<MangaResponse?> GetMangaByUrlAsync(string url);
    Task<ParsingResponse> CreateParsingTicketAsync(string mangaUrl, string callback);
}