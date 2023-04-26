namespace Nude.API.Infrastructure.Services.Background;

public interface IBackgroundWorker
{
    Task ExecuteAsync(BackgroundServiceContext ctx, CancellationToken ctk);
    Task HandleExceptionAsync(Exception exception);
}