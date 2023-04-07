using Nude.API.Contracts.Manga.Responses;

namespace Nude.Bot.Tg.Clients.Telegraph;

public interface ITelegraphClient
{
    Task<string?> UploadFileAsync(string externalFileUrl);
    Task<string> CreatePageAsync(MangaResponse manga);
}