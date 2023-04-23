using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Nude.API.Infrastructure.Initializers;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync<TContext>(IServiceProvider services)
    where TContext : DbContext
    {
        await using var scope = services.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<TContext>();
        await context.Database.MigrateAsync();
    }
}