using Microsoft.Extensions.Configuration;
using Nude.API.Infrastructure.Services.Storages.Results;

namespace Nude.API.Infrastructure.Services.Storages;

public class FileLocalStorage : IFileStorage
{
    private readonly string _basePath;
    private readonly string _baseUrl;

    // TODO: IOptions<LocalStorageConfig>
    public FileLocalStorage(IConfiguration configuration)
    {
        _basePath = configuration["Storage:BasePath"];
        _baseUrl = configuration["Storage:BaseUrlPath"];
        Directory.CreateDirectory(_basePath);
    }
    
    public async Task<FileSavingResult> SaveAsync(byte[] data, string mimeType)
    {
        var fileName = GenerateFileName(mimeType);
        var savePath = GetSavePath(fileName);
        
        await File.WriteAllBytesAsync(savePath, data);

        return new FileSavingResult
        {
            Url = GetFileUrl(fileName)
        };
    }

    private string GetSavePath(string fileName) => _basePath + "/" + fileName;
    
    private string GetFileUrl(string fileName) => "http://127.0.0.1:3001" + _baseUrl + "/" + fileName;

    private static string GenerateFileName(string mimeType)
    {
        const string imageType = "image";
        var name = Guid.NewGuid().ToString().Replace("-", "");
        
        if (mimeType.Contains(imageType))
        {
            var extension = mimeType.Split("/").Last();
            extension = extension == imageType
                ? ".png"
                : "." + extension;

            return name + extension;
        }

        throw new NotImplementedException();
    }
}