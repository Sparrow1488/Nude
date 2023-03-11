namespace Nude.API.Infrastructure.Services.Background;

public interface IBackgroundWorker
{
    Task ExecuteAsync(BackgroundServiceContext context, CancellationToken ctk);
}