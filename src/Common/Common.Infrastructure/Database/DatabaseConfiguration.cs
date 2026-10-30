using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.Database;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddModuleDbContext<TDbContext>(
        this IServiceCollection services,
        string connectionString,
        string schema)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(options => options
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsHistoryTable(HistoryRepository.DefaultTableName, schema))
            .UseSnakeCaseNamingConvention());

        services.AddSingleton(new ModuleDatabase(typeof(TDbContext)));

        return services;
    }

    public static async Task ApplyModuleMigrationsAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        foreach (var database in scope.ServiceProvider.GetServices<ModuleDatabase>())
        {
            var dbContext = (DbContext)scope.ServiceProvider.GetRequiredService(database.DbContextType);
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
    }

    private sealed record ModuleDatabase(Type DbContextType);
}
