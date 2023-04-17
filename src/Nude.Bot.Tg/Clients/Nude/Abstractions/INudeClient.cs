using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Tokens.Responses;
using Nude.API.Models.Formats;
using Nude.Bot.Tg.Models.Api;

namespace Nude.Bot.Tg.Clients.Nude.Abstractions;

public interface INudeClient : IAuthorizedClientCreator
{
    Task<ApiResult<JwtTokenResponse>> AuthorizeAsync(string username);
    Task<ApiResult<MangaResponse>> FindMangaByUrlAsync(string sourceUrl, FormatType? format = null);
    Task<ApiResult<MangaResponse>> FindMangaByContentKeyAsync(string contentKey, FormatType? format = null);
    Task<ApiResult<MangaResponse>> GetRandomMangaAsync(FormatType? format = null);
}