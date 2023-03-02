using Microsoft.Extensions.DependencyInjection;
using Nude.API.Infrastructure.Services.Background;
using Quartz;

namespace Nude.API.Infrastructure.Extensions;

public static class BackgroundServiceExtensions
{
    public static IServiceCollection AddBgService<TBgService>(
        this IServiceCollection services, 
        string name
    ) where TBgService : BgService
    {
        services.AddQuartz(q =>
        {
            #pragma warning disable CS0618
            q.UseMicrosoftDependencyInjectionScopedJobFactory();
    
            var jobKey = new JobKey(name);
            q.AddJob<TBgService>(opts => opts.WithIdentity(jobKey));
    
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(name + " Identity")
                .WithSchedule(SimpleScheduleBuilder.Create())
            );
        });

        return services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = false;
        });
    }
}