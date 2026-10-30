using Basket.Application.Abstractions;
using Basket.Application.Baskets;
using Basket.Infrastructure.Baskets;
using Basket.Infrastructure.Database;
using Common.Infrastructure.Database;
using Common.Presentation.Endpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.Infrastructure;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string 'Database' is not configured.");

        services.AddModuleDbContext<BasketDbContext>(connectionString, BasketDbContext.Schema);

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<BasketDbContext>());
        services.AddScoped<IBasketRepository, BasketRepository>();

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }
}
