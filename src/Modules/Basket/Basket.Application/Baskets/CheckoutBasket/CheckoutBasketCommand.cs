using Common.Application.Messaging;

namespace Basket.Application.Baskets.CheckoutBasket;

internal sealed record CheckoutBasketCommand(Guid CustomerId) : ICommand<Guid>;
