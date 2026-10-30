using Common.Application.Messaging;
using Common.Domain;

namespace Basket.Application.Baskets.GetBasket;

internal sealed class GetBasketQueryHandler(IBasketRepository basketRepository)
    : IQueryHandler<GetBasketQuery, BasketResponse>
{
    public async Task<Result<BasketResponse>> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetByCustomerIdAsync(query.CustomerId, cancellationToken);

        return basket is null
            ? BasketResponse.Empty(query.CustomerId)
            : BasketResponse.FromBasket(basket);
    }
}
