namespace Nude.API.Infrastructure.Services.Background;

public class BackgroundWorkersBuffer
{
    public ICollection<Type> WorkersTypes { get; } = new List<Type>();
}