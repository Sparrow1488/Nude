using Nude.API.Contracts.Images.Responses;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Tokens.Responses;
using Nude.API.Models.Formats;
using Nude.Bot.Tg.Models.Api;

namespace Nude.Bot.Tg.Clients.Nude.Abstractions;

public interface INudeClient : IAuthorizedClientCreator
{
    Task<ApiResult<JwtTokenResponse>> AuthorizeAsync(string username);
    Task<ApiResult<ImageResponse[]>> FindImagesByTagsAsync(IEnumerable<string> tags);
    Task<ApiResult<MangaResponse>> FindMangaByUrlAsync(string sourceUrl, FormatType? format = null);
    Task<ApiResult<MangaResponse>> FindMangaByContentKeyAsync(string contentKey, FormatType? format = null);
    Task<ApiResult<ImageResponse[]>> GetRandomImagesAsync(int count = 5);
}