using Catalog.Application.Abstractions;
using Catalog.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Database;

internal sealed class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options), IUnitOfWork
{
    public const string Schema = "catalog";

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }
}
