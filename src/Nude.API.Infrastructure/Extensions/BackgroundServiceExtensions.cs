using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nude.API.Infrastructure.Services.Background;

namespace Nude.API.Infrastructure.Extensions;

public static class BackgroundServiceExtensions
{
    public static IServiceCollection AddBackgroundWorkers(
        this IServiceCollection services, params Type[] workers)
    {
        services.AddHostedService<LoopBackgroundService>();
        services.TryAddSingleton<BackgroundWorkerTypesProvider>(_ =>
        {
            var buffer = new BackgroundWorkerTypesProvider();
            buffer.WorkerTypes.AddRange(workers);

            return buffer;
        });
        return services;
    }
}