using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nude.API.Infrastructure.Services.Seeds;

namespace Nude.API.Infrastructure.Initializers;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync<TContext>(IServiceProvider services, IDataSeeder? seeder = null)
    where TContext : DbContext
    {
        await using var scope = services.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<TContext>();
        await context.Database.MigrateAsync();

        if (seeder is not null)
        {
            await seeder.SeedDataAsync();
        }
    }
}