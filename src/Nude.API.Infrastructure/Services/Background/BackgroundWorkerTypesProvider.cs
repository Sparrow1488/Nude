namespace Nude.API.Infrastructure.Services.Background;

public class BackgroundWorkerTypesProvider
{
    public ICollection<Type> WorkerTypes { get; } = new List<Type>();
}