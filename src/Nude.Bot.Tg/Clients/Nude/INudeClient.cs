using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Models.Formats;
using Nude.Bot.Tg.models.Api;

namespace Nude.Bot.Tg.Clients.Nude;

public interface INudeClient
{
    Task<ApiResult<MangaResponse>?> FindMangaByUrlAsync(string sourceUrl, FormatType? format = null);
    Task<ApiResult<MangaResponse>?> FindMangaByContentKeyAsync(string contentKey, FormatType? format = null);
    Task<ApiResult<MangaResponse>?> GetRandomMangaAsync(FormatType? format = null);
    Task<ApiResult<ContentTicketResponse>?> CreateContentTicket(ContentTicketRequest request);
}