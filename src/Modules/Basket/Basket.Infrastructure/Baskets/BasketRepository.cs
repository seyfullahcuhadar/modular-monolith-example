using Basket.Application.Baskets;
using Basket.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Basket.Infrastructure.Baskets;

internal sealed class BasketRepository(BasketDbContext dbContext) : IBasketRepository
{
    public Task<Domain.Baskets.Basket?> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default) =>
        dbContext.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.Id == customerId, cancellationToken);

    public void Add(Domain.Baskets.Basket basket) => dbContext.Baskets.Add(basket);
}
