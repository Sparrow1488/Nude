using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Nude.API.Infrastructure.Services.Background;

public class ForeverBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ForeverBackgroundService> _logger;

    public ForeverBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<ForeverBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var context = new BackgroundServiceContext
        {
            Delay = TimeSpan.FromSeconds(5)
        };

        await using var scope = _serviceProvider.CreateAsyncScope();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var workers = scope.ServiceProvider.GetServices<IBackgroundWorker>().ToArray();

            if (workers.Length == 0)
            {
                _logger.LogInformation("No any background service worker registered");
                return;
            }
            
            var tasks = workers.Select(x => x.ExecuteAsync(context, stoppingToken));
            await Task.WhenAll(tasks.ToArray());

            await Task.Delay(context.Delay, stoppingToken);
        }
    }
}