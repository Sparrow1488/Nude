using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nude.API.Infrastructure.Services.Background;

namespace Nude.API.Infrastructure.Extensions;

public static class BackgroundServiceExtensions
{
    public static IServiceCollection AddBackgroundWorker<TWorker>(this IServiceCollection services)
        where TWorker : class, IBackgroundWorker
    {
        services.TryAddSingleton<BackgroundWorkersBuffer>();
        services.AddHostedService<LoopBackgroundService>();
        return services.AddScoped<IBackgroundWorker, TWorker>();
    }
    
    public static IServiceCollection AddBackgroundWorkers(
        this IServiceCollection services, params Type[] workers)
    {
        services.AddHostedService<LoopBackgroundService>();
        services.TryAddSingleton<BackgroundWorkersBuffer>(_ =>
        {
            var buffer = new BackgroundWorkersBuffer();
            buffer.WorkersTypes.AddRange(workers);

            return buffer;
        });
        return services;
    }
}