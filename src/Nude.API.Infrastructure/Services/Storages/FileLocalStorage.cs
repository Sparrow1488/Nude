using Microsoft.Extensions.Configuration;
using Nude.API.Infrastructure.Services.Storages.Results;

namespace Nude.API.Infrastructure.Services.Storages;

public class FileStorage : IFileStorage
{
    private readonly string _basePath;

    public FileStorage(IConfiguration configuration)
    {
        _basePath = configuration["Storage:BasePath"];
        Directory.CreateDirectory(_basePath);
    }
    
    public async Task<FileSavingResult> SaveAsync(byte[] data, string mimeType)
    {
        var fileName = GenerateFileName(mimeType);
        var savePath = GetSavePath(fileName);
        
        await using var fileStream = File.Create(savePath);
        await File.WriteAllBytesAsync(savePath, data);

        return new FileSavingResult
        {
            IsSuccess = true,
            Url = "http://127.0.0.1:3001/" + fileName
        };
    }

    private string GetSavePath(string fileName)
    {
        return _basePath + "/" + fileName;
    }

    private static string GenerateFileName(string mimeType)
    {
        const string imageType = "image";
        var name = Guid.NewGuid().ToString().Replace("-", "");
        
        if (mimeType.Contains(imageType))
        {
            var extension = mimeType.Split("/").Last();
            extension = extension == imageType
                ? ".png"
                : extension;

            return name + extension;
        }

        throw new NotImplementedException();
    }
}