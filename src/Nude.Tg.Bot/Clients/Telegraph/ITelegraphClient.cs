using Nude.API.Contracts.Manga.Responses;

namespace Nude.Tg.Bot.Clients.Telegraph;

public interface ITelegraphClient
{
    Task<string?> UploadFileAsync(string externalFileUrl);
    Task<string> CreatePageAsync(MangaResponse manga);
}