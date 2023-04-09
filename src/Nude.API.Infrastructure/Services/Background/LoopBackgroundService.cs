using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Nude.API.Infrastructure.Services.Background;

public class LoopBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly BackgroundWorkersBuffer _workersBuffer;
    private readonly ILogger<LoopBackgroundService> _logger;

    public LoopBackgroundService(
        IServiceProvider serviceProvider,
        BackgroundWorkersBuffer workersBuffer,
        ILogger<LoopBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _workersBuffer = workersBuffer;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var context = new BackgroundServiceContext
        {
            Delay = TimeSpan.FromSeconds(2)
        };
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var scopes = new List<AsyncServiceScope>();
            var workers = new List<IBackgroundWorker>();
            
            foreach (var type in _workersBuffer.WorkersTypes)
            {
                var scope = _serviceProvider.CreateAsyncScope();
                var worker = (IBackgroundWorker) ActivatorUtilities.CreateInstance(scope.ServiceProvider, type);

                scopes.Add(scope);
                workers.Add(worker);
            }

            if (workers.Count == 0)
            {
                _logger.LogInformation("No any background service worker registered");
                return;
            }
            
            var tasks = workers.Select(
                x => x.ExecuteAsync(context, stoppingToken)
            ).ToArray();
            
            Task.WaitAll(tasks, stoppingToken);

            scopes.ForEach(x => x.Dispose());
            await Task.Delay(context.Delay, stoppingToken);
        }
    }
}