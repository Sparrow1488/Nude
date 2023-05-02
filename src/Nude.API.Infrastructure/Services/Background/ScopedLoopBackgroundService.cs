using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Nude.API.Infrastructure.Services.Background;

public sealed class ScopedLoopBackgroundService<TWorker> : BackgroundService
where TWorker : IBackgroundWorker
{
    public ScopedLoopBackgroundService(
        IServiceProvider services,
        ILogger<ScopedLoopBackgroundService<TWorker>> logger)
    {
        Services = services;
        Logger = logger;
    }

    private IServiceProvider Services { get; }
    private ILogger<ScopedLoopBackgroundService<TWorker>> Logger { get; }

    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = Services.CreateAsyncScope();
            
            var worker = ActivatorUtilities.CreateInstance<TWorker>(scope.ServiceProvider);
            var workerName = worker.GetType().Name;

            var context = DefaultContext();
            
            try
            {
                Logger.LogInformation("{worker} job started", workerName);

                await worker.ExecuteAsync(context, stoppingToken);
            }
            catch (Exception exception)
            {
                // TODO: it's not executed because caught in bg-worker
                await worker.HandleExceptionAsync(exception);
            }
            finally
            {
                Logger.LogInformation("{worker} job ended", workerName);
            }
            
            await Task.Delay(context.LoopDelay, stoppingToken);
        }
    }

    private static BackgroundServiceContext DefaultContext()
    {
        return new BackgroundServiceContext
        {
            LoopDelay = TimeSpan.FromSeconds(2)
        };
    }
}