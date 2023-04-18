using Nude.Bot.Tg.Clients.Nude.Abstractions;
using Nude.Bot.Tg.Services.Users;
using Nude.Bot.Tg.Telegram.Endpoints.Base;
using Telegram.Bot;

namespace Nude.Bot.Tg.Telegram.Endpoints.Update.Pictures;

public class PictureUploadEndpoint : TelegramUpdateEndpoint
{
    private readonly INudeClient _client;

    public PictureUploadEndpoint(INudeClient client)
    {
        _client = client;
    }
    
    public override async Task HandleAsync()
    {
        var sizes = Update.Message!.Photo!;
        var uploadSize = sizes.Last();
        
        await using var memory = new MemoryStream();
        var file = await BotClient.GetFileAsync(uploadSize.FileId);
        if (file.FilePath == null)
        {
            throw new Exception("Telegram FilePath is null");
        }
        
        await BotClient.DownloadFileAsync(file.FilePath, memory);

        memory.Seek(0, SeekOrigin.Begin);
        var bytes = memory.ToArray();

        var authorizeClient = _client.AuthorizeClient(UserSession);
        var result = await authorizeClient.CreateImageAsync(
            bytes,
            file.FilePath
        );

        if (result.IsSuccess)
        {
            await MessageAsync("Успешно загружено: " + result.ResultValue.Url);
        }
        else
        {
            await MessageAsync(result.Status + ": " + result.Message);
        }
    }

    public override bool CanHandle() => Update.Message?.Photo != null;
}