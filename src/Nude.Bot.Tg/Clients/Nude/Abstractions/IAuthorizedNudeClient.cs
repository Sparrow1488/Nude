using Nude.API.Contracts.Images.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.Bot.Tg.Models.Api;

namespace Nude.Bot.Tg.Clients.Nude.Abstractions;

public interface IAuthorizedNudeClient : INudeClient
{
    Task<ApiResult<ContentTicketResponse>> CreateContentTicketAsync(ContentTicketRequest request);
    Task<ApiResult<ImageResponse>> CreateImageAsync(byte[] data, string filePath);
}