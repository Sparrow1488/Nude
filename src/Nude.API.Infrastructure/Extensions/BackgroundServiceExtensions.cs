using Microsoft.Extensions.DependencyInjection;
using Nude.API.Infrastructure.Services.Background;

namespace Nude.API.Infrastructure.Extensions;

public static class BackgroundServiceExtensions
{
    public static IServiceCollection AddBackgroundWorker<TWorker>(this IServiceCollection services)
    where TWorker : class, IBackgroundWorker
    {
        services.AddHostedService<ForeverBackgroundService>();
        return services.AddScoped<IBackgroundWorker, TWorker>();
    }
}