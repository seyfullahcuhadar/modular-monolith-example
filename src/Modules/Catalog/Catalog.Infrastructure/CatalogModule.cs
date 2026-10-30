using Catalog.Application.Abstractions;
using Catalog.Application.Products;
using Catalog.Infrastructure.Database;
using Catalog.Infrastructure.Products;
using Catalog.Infrastructure.PublicApi;
using Catalog.PublicApi;
using Common.Infrastructure.Database;
using Common.Presentation.Endpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string 'Database' is not configured.");

        services.AddModuleDbContext<CatalogDbContext>(connectionString, CatalogDbContext.Schema);

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CatalogDbContext>());
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICatalogApi, CatalogApi>();

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }
}
