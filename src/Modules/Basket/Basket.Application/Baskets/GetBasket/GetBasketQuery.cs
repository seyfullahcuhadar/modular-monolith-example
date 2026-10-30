using Common.Application.Messaging;

namespace Basket.Application.Baskets.GetBasket;

internal sealed record GetBasketQuery(Guid CustomerId) : IQuery<BasketResponse>;
