using Quartz;

namespace Nude.API.Infrastructure.Services.Background;

public abstract class BgService : IJob
{
    protected abstract Task ExecuteAsync();
    
    public Task Execute(IJobExecutionContext context)
    {
        return ExecuteAsync();
    }
}