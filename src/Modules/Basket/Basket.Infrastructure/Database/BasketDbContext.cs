using Basket.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Basket.Infrastructure.Database;

internal sealed class BasketDbContext(DbContextOptions<BasketDbContext> options) : DbContext(options), IUnitOfWork
{
    public const string Schema = "basket";

    public DbSet<Domain.Baskets.Basket> Baskets => Set<Domain.Baskets.Basket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BasketDbContext).Assembly);
    }
}
