using Nude.API.Contracts.Blacklists.Responses;
using Nude.API.Contracts.Images.Responses;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Tags.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Models.Formats;
using Nude.Bot.Tg.Models.Api;

namespace Nude.Bot.Tg.Clients.Nude.Abstractions;

public interface IAuthorizedNudeClient : INudeClient
{
    Task<ApiResult<ContentTicketResponse>> CreateContentTicketAsync(ContentTicketRequest request);
    Task<ApiResult<ImageResponse>> CreateImageAsync(byte[] data, string filePath);
    Task<ApiResult<MangaResponse>> GetRandomMangaAsync(FormatType? format = null);
    Task<ApiResult<BlacklistResponse>> GetBlacklistAsync();
    Task<ApiResult<BlacklistResponse>> SetDefaultBlacklistAsync();
    Task<ApiResult<TagResponse[]>> SetBlacklistTagsAsync(params string[] tags);
    Task<ApiResult<TagResponse[]>> DeleteBlacklistTagsAsync(params string[] tags);
}