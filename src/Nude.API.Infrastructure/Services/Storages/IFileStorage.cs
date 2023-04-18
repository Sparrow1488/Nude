using Nude.API.Infrastructure.Services.Storages.Results;

namespace Nude.API.Infrastructure.Services.Storages;

public interface IFileStorage
{
    Task<FileSavingResult> SaveAsync(byte[] data, string mimeType);
}