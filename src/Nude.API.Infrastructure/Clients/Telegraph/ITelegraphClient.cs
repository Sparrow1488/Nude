namespace Nude.API.Infrastructure.Clients.Telegraph;

public interface ITelegraphClient
{
    Task<string?> UploadFileAsync(string externalFileUrl);
    Task<string> CreatePageAsync(
        string title,
        string text,
        IEnumerable<string> images);
}