namespace Basket.Application.Baskets;

internal interface IBasketRepository
{
    Task<Domain.Baskets.Basket?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);

    void Add(Domain.Baskets.Basket basket);
}
